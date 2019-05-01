using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ENT = Josheph.Framework.Entity;
using BAL = Josheph.Framework.BusinessLayer;
using HtmlAgilityPack;

namespace Josheph.Framework.BusinessLayer
{
    public class SMTPManagement
    {
        private static List<ENT.PlantMasterSUB> lstPlant = new List<ENT.PlantMasterSUB>();
        List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
        private const string _FromMailAddress = "reporting.ambit@gmail.com";
        private const string _SMTPServer = "smtp.gmail.com";
        private const int _SMTPPort = 587;
        private const string _Username = "reporting.ambit";
        private const string _Password = "Energy*1111";

        public string ToMailID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailTemplate { get; set; }
        public decimal TodayEnergyProduced{ get; set; }

        public SMTPManagement()
        {
            lstPlant = new PlantMasterBAL().GetAll(string.Empty);
        }

        public bool SendPlantDailyMail()
        {
            bool blnResult = false;
            try
            {
                foreach (ENT.PlantMasterSUB el in lstPlant)
                {
                    if (el.Status == Common.MyEnumration.MyStatus.Active)
                    {
                        this.Body = GenerateHtmlTemplate(el.PlantId.ToString());
                        this.ToMailID = el.EmailId;
                        this.Subject = "Plant Status " + " ( " + DateTime.Now.ToString("dd/MM/yyyy") + " ) " + el.PlantName;
                        
                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(this.Body);

                        HtmlNodeCollection eptd = htmlDocument.DocumentNode.SelectNodes("//td");

                        String ept = eptd[1].InnerHtml.ToString();

                        Console.WriteLine("Production For Plant : "+el.PlantName + " is "+ ept);

                        // send message and mail for plant
                        
                        SendEmail();
                        
                        // added custome email for specific plant.
                        if (el.PlantId == Guid.Parse("E91BE008-E124-406F-94CD-E910BC5F19CD"))
                        {
                            this.ToMailID = "accounts1@esteemauto.com";
                            SendEmail();
                        }

                        SendMessage(el.Mobile, ept, el.PlantName);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return blnResult;
        }

        private string SendEmail()
        {
            string ReturnMessage = "";
            if (lstPlant.Count > 0)
            {
                try
                {
                    MailMessage mailMessage = new MailMessage(_FromMailAddress, this.ToMailID, this.Subject, this.Body);
                    mailMessage.CC.Add("generation.ambit1@gmail.com");
                    SmtpClient smtpClient = new SmtpClient(_SMTPServer.ToLower(), _SMTPPort);
                    smtpClient.Credentials = new NetworkCredential(_Username.ToLower(), _Password);
                    smtpClient.EnableSsl = true;
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Send(mailMessage);
                    ReturnMessage = "E-mail sent Successfully.";
                }
                catch (Exception _Exception)
                {
                    ReturnMessage = "Could not send the e-mail - error: " + _Exception.Message;
                }
            }
            return ReturnMessage;
        }

        private string GenerateHtmlTemplate(string ID)
        {
            var baseUrl = "http://infotronix.appsmith.co.in";// string.Format("{0}://{1}{2}", request1.Url.Scheme, request1.Url.Authority, appUrl);
            string html = string.Empty;
            string url;
            url = baseUrl + "/PlantMaster/GetPlantReport/" + ID;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                html = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return html;
        }
        
        private string SendMessage(String mobile,String EP , String PlantName)
        {
            string ReturnMessage = "";
            try
            {
                string url;

                String msg = "Today's Total Production for Plant : " + PlantName + " is " + EP + " .";
                
                url = "http://sms.versatilesmshub.com/api/mt/SendSMS?user=CSKInfotronix&password=CSK@123&senderid=SOLARX&channel=Trans&DCS=0&flashsms=0&number=9426666404," + mobile+"&text="+msg+"&route=1";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string strResponse = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (BAL.MessageSendLog objBal = new MessageSendLog())
                    {
                        objBal.Entity.IsSent = 1;
                        objBal.Entity.Mobile = mobile;
                        objBal.Entity.Response = strResponse;
                        objBal.Entity.CreatedDateTime = DateTime.Now;
                        objBal.Entity.SystemDateTime = DateTime.Now;
                        objBal.Entity.CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        objBal.Insert(objBal.Entity);
                    }
                    ReturnMessage = "Message sent Successfully";
                }
                else
                {
                    using (BAL.MessageSendLog objBal = new MessageSendLog())
                    {
                        objBal.Entity.IsSent = 1;
                        objBal.Entity.Mobile = mobile;
                        objBal.Entity.Response = strResponse;
                        objBal.Entity.CreatedDateTime = DateTime.Now;
                        objBal.Entity.SystemDateTime = DateTime.Now;
                        objBal.Entity.CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        objBal.Insert(objBal.Entity);
                    }
                    throw new Exception("Unable to send message due to error: " + response.StatusDescription);
                }
            }
            catch (Exception _Exception)
            {
                ReturnMessage = "Could not send the message - error: " + _Exception.Message;

                using (BAL.MessageSendLog objBal = new MessageSendLog())
                {
                    objBal.Entity.IsSent = 1;
                    objBal.Entity.Mobile = mobile;
                    objBal.Entity.Response = _Exception.Message;
                    objBal.Insert(objBal.Entity);
                }
                throw new Exception("Unable to send message due to error: " + _Exception.Message);
            }
            return ReturnMessage;
        }

        private decimal GetTodayProduction(String PlantId)
        {
            decimal production = 0;

            try
            {
                using (BAL.DeviceDataBAL objDashboard = new BAL.DeviceDataBAL())
                {
                    if (PlantId == "654F9AC4-601D-4530-BB54-E667D037B1F3")
                    {
                        lstResult = objDashboard.GetDashboardCardsExcluded(true, false, Guid.Parse(PlantId), "('1900763842','1900764245')");

                        List<ENT.DashboardCards> lstResultTop = new List<ENT.DashboardCards>();
                        lstResultTop = objDashboard.GetDashboardCardsTop(true, true, Guid.Parse(PlantId), "('1900763842','1900764245')");

                        lstResult = lstResult.Concat(lstResultTop).ToList<ENT.DashboardCards>();
                    }
                    else
                    {
                        lstResult = objDashboard.GetDashboardCards(true, false, Guid.Parse(PlantId));
                    }
                    production = lstResult.Sum(x => x.EAC);
                }
            }
            catch (Exception ex)
            {

            }

            return production;
        }

        public string SendForgetPasswordOTP(string EmailID, int OTP)
        {
            string blnResult = "";
            try
            {
                this.Body = "Forget Password OTP is : " + OTP.ToString();
                this.ToMailID = EmailID;
                this.Subject = "Forget Password OTP";
                MailMessage mailMessage = new MailMessage(_FromMailAddress, this.ToMailID, this.Subject, this.Body);
                SmtpClient smtpClient = new SmtpClient(_SMTPServer.ToLower(), _SMTPPort);
                smtpClient.Credentials = new NetworkCredential(_Username.ToLower(), _Password);
                smtpClient.EnableSsl = true;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                blnResult = "E-mail sent Successfully";
            }
            catch (Exception _Exception)
            {
                blnResult = "Could not send the e-mail - error: " + _Exception.Message;
            }
            return blnResult;
        }
    }
}
