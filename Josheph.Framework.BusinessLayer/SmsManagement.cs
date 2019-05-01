using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ENT = Josheph.Framework.Entity;
using BAL = Josheph.Framework.BusinessLayer;
using System.IO;

namespace Josheph.Framework.BusinessLayer
{
    public class SmsManagement
    {
        public SmsManagement()
        {
            
        }

        public string SendMessage(String mobile, String msg)
        {
            string ReturnMessage = "";
            try
            {
                string url;
                // url = "http://sms.jajasms.com/submitsms.jsp?user=shreemkt&key=33aaa719e5XX&mobile=" + mobile + "&message=" + msg + "&senderid=SOLARX&accusage=1";

                url = "http://sms.versatilesmshub.com/api/mt/SendSMS?user=CSKInfotronix&password=CSK@123&senderid=SOLARX&channel=Trans&DCS=0&flashsms=0&number=9426666404,8320399766," + mobile + "&text=" + msg + "&route=1";
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
                        objBal.Insert(objBal.Entity);
                    }
                    ReturnMessage = "Message sent Successfully";
                }
                else
                {
                    using (BAL.MessageSendLog objBal = new MessageSendLog())
                    {
                        objBal.Entity.IsSent = 2;
                        objBal.Entity.Mobile = mobile;
                        objBal.Entity.Response = strResponse;
                        objBal.Entity.CreatedDateTime = DateTime.Now;
                        objBal.Entity.SystemDateTime = DateTime.Now;
                        objBal.Insert(objBal.Entity);
                    }
                    throw new Exception("Unable to send message due to error: " + response.StatusDescription);
                }
            }
            catch (Exception _Exception)
            {
                using (BAL.MessageSendLog objBal = new MessageSendLog())
                {
                    objBal.Entity.IsSent = 1;
                    objBal.Entity.Mobile = mobile;
                    objBal.Entity.Response = _Exception.Message;
                    objBal.Entity.CreatedDateTime = DateTime.Now;
                    objBal.Entity.SystemDateTime = DateTime.Now;
                    objBal.Insert(objBal.Entity);
                }
                ReturnMessage = "Could not send the message - error: " + _Exception.Message;
            }
            return ReturnMessage;
        }
    }
}
