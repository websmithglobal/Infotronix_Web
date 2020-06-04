using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infotronix
{
    public class PlantEmailLogic
    {
        private const string FromEmail = "reporting.ambit@gmail.com";
        private const string SMTPServer = "smtp.gmail.com";
        private const int PortNo = 587;
        private const string UserName = "reporting.ambit";
        private const string Password = "Energy*1111";

        public bool SendPlantEmail()
        {
            bool blnResult = false;

            try
            {
                DataTable dt = new DBLogic().PlantMaster_GetAll();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string EmailPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                            + "/EmailTemplate/EmailTemplate.html";

                        StreamReader reader = new StreamReader(EmailPath);
                        string Data = reader.ReadToEnd();
                        reader.Close();

                        string EmailBody = Data;

                        StringBuilder sb = new StringBuilder();

                        Guid PlantID = new Guid();
                        Guid.TryParse(dt.Rows[i]["PlantId"].ToString(), out PlantID);

                        DataSet dsPlantDetail = new DBLogic().PlantMaster_GetForSendEmail(PlantID);

                        if (dsPlantDetail.Tables.Count > 0)
                        {
                            DataTable dtPlant = dsPlantDetail.Tables[0];

                            if (dtPlant.Rows.Count > 0)
                            {
                                string PlantName = dtPlant.Rows[0]["PlantName"].ToString();

                                double TotalEAC = 0;

                                if (PlantID.ToString() == "c91b5d34-a7c5-4e79-b211-5ae00e72eb43")
                                {
                                    // DEVICE START
                                    DataTable dtEAC1 = dsPlantDetail.Tables[1];

                                    DataTable dtEAC2 = dsPlantDetail.Tables[2];

                                    List<DeviceClass> lstResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceClass>>(
                                        Newtonsoft.Json.JsonConvert.SerializeObject(dtEAC1));

                                    List<DeviceClass> lstResultTop = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceClass>>(
                                        Newtonsoft.Json.JsonConvert.SerializeObject(dtEAC2));

                                    lstResult = lstResult.Concat(lstResultTop).ToList<DeviceClass>();

                                    TotalEAC = lstResult.Sum(x => x.EAC);
                                    // DEVICE END

                                    // DEVICE DAY START
                                    DataTable dtEACDay1 = dsPlantDetail.Tables[3];

                                    DataTable dtEACDay2 = dsPlantDetail.Tables[4];

                                    List<DeviceDayClass> lstTemp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceDayClass>>(
                                        Newtonsoft.Json.JsonConvert.SerializeObject(dtEACDay1));

                                    List<DeviceDayClass> lstTempTop = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeviceDayClass>>(
                                        Newtonsoft.Json.JsonConvert.SerializeObject(dtEACDay2));

                                    lstTemp = lstTemp.Concat(lstTempTop).ToList<DeviceDayClass>();

                                    List<DeviceDayClass> lstTable = lstTemp;
                                    // DEVICE DAY END

                                    for (int j = 0; j < lstTable.Count; j++)
                                    {
                                        sb.Append(GetTableRow(lstTable[j].DeviceName
                                            , lstTable[j].Day1.ToString()));
                                    }
                                }
                                else
                                {
                                    DataTable dtEAC = dsPlantDetail.Tables[1];

                                    for (int j = 0; j < dtEAC.Rows.Count; j++)
                                    {
                                        double.TryParse(dtEAC.Rows[j]["EAC"].ToString(), out double mEAC);

                                        TotalEAC = TotalEAC + mEAC;
                                    }

                                    DataTable dtEACDetail = dsPlantDetail.Tables[2];

                                    for (int j = 0; j < dtEACDetail.Rows.Count; j++)
                                    {
                                        sb.Append(GetTableRow(dtEACDetail.Rows[j]["DeviceName"].ToString()
                                            , dtEACDetail.Rows[j]["Day1"].ToString()));
                                    }
                                }

                                EmailBody =
                                    (((EmailBody.Replace("{PlantName}", PlantName))
                                    .Replace("{Date}", "Date " + DateTime.Now.ToString("dd/MM/yyyy")))
                                    .Replace("{TotalEAC}", "Energy Produced - Today " + TotalEAC.ToString("##0.0000") + " kWh"))
                                    .Replace("{RepeatData}", sb.ToString());

                                string ToEmailID = dt.Rows[i]["EmailId"].ToString();

                                string MobileNo = dt.Rows[i]["Mobile"].ToString();

                                string Subject = "Plant Status " + " ( " + DateTime.Now.ToString("dd/MM/yyyy") + " ) " + PlantName;

                                Console.WriteLine("Production For Plant : " + PlantName + " is " + TotalEAC);

                                SendEmail(ToEmailID, Subject, EmailBody);

                                SendMessage(MobileNo, TotalEAC.ToString(), PlantName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return blnResult;
        }

        private string SendEmail(string ToEmailID, string Subject, string Body)
        {
            string ReturnMessage = string.Empty;

            try
            {
                MailMessage mailMessage = new MailMessage(FromEmail, ToEmailID, Subject, Body);
                mailMessage.CC.Add("generation.ambit1@gmail.com");
                //mailMessage.CC.Add("csk.servicedesk2@gmail.com");
                SmtpClient smtpClient = new SmtpClient(SMTPServer, PortNo)
                {
                    Credentials = new NetworkCredential(UserName, Password),
                    EnableSsl = true
                };
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
                ReturnMessage = "E-mail sent Successfully.";
            }
            catch (Exception _Exception)
            {
                ReturnMessage = "Could not send the e-mail - error: " + _Exception.Message;
            }

            return ReturnMessage;
        }

        private string SendMessage(string MobileNo, string EP, string PlantName)
        {
            string ReturnMessage = string.Empty;

            try
            {
                string url;

                String msg = "Today's Total Production for Plant : " + PlantName + " is " + EP + ".";

                url = "http://sms.versatilesmshub.com/api/mt/SendSMS?user=websmithoffice&password=Web@12345&senderid=WEBSMH&channel=Trans&DCS=0&flashsms=0&number=9426666404," + MobileNo + "&text=" + msg + "&route=4";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string strResponse = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    new DBLogic().MessageSendLog_Insert(MobileNo, strResponse, 1);

                    ReturnMessage = "Message sent Successfully";

                    Console.WriteLine(ReturnMessage);
                }
                else
                {
                    new DBLogic().MessageSendLog_Insert(MobileNo, strResponse, 0);

                    ReturnMessage = "Unable to send message due to error: " + response.StatusDescription;

                    Console.WriteLine(ReturnMessage);
                }
            }
            catch (Exception _Exception)
            {
                new DBLogic().MessageSendLog_Insert(MobileNo, _Exception.Message, 0);

                ReturnMessage = "Could not send the message - error: " + _Exception.Message;

                Console.WriteLine(ReturnMessage);
            }

            return ReturnMessage;
        }

        public string GetTableRow(string DeviceName, string Energy)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>");
            sb.Append("<td style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; border-top-width: 1px; border-top-color: #eee; border-top-style: solid; margin: 0; padding: 5px 0;' valign='top'>");
            sb.Append(DeviceName);
            sb.Append("</td>");
            sb.Append("<td class='alignright' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; text-align: right; border-top-width: 1px; border-top-color: #eee; border-top-style: solid; margin: 0; padding: 5px 0;' align='right' valign='top'>");
            sb.Append(Energy);
            sb.Append("</td>");
            sb.Append("</tr>");

            return sb.ToString();
        }
    }

    public class DeviceClass
    {
        public string SerialNo { get; set; }
        public double EAC { get; set; }
    }

    public class DeviceDayClass
    {
        public string DeviceName { get; set; }
        public double Day1 { get; set; }
    }
}