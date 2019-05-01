using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infotronix.TestApp
{
    public class CommanClass
    {
        #region Other Fun
        /// <summary>
        /// Function to return zero if null.
        /// </summary>
        /// <param name="Val"></param>
        /// <returns></returns>
        public static string ReturnZero(string Val)
        { return string.IsNullOrEmpty(Val) ? "0" : Val; }
        /// <summary>
        /// Function to return blank if null.
        /// </summary>
        /// <param name="Val"></param>
        /// <returns></returns>
        public static string ReturnBlank(string Val)
        { return string.IsNullOrEmpty(Val) ? "" : Val; }
        /// <summary>
        /// Function to return "00000000-0000-0000-0000-000000000000" if null.
        /// </summary>
        /// <param name="Val"></param>
        /// <returns></returns>
        public static string ReturnUniqueID(string Val)
        { return string.IsNullOrEmpty(Val) ? "00000000-0000-0000-0000-000000000000" : Val.Trim(); }
        #endregion

        // <summary>
        /// Send mail when get any error.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        public static string SendMail(Exception ex, string SubDeviceID)
        {
            string RetStr = string.Empty;
            try
            {
                DataTable dtFpt = SqlHelper.ExecuteProcedure("select top 1 EmailID,password,ToEmialID from EmailMaster where IsStatus = 1");
                //string ftpServerIP = dtFpt.Rows[0]["ftpip"].ToString(), ftpUserID = dtFpt.Rows[0]["userid"].ToString(), ftpPassword = dtFpt.Rows[0]["password"].ToString();

                string FromEmailID = dtFpt.Rows[0]["EmailID"].ToString(), ToEmailID = dtFpt.Rows[0]["ToEmialID"].ToString(), MailUserID = dtFpt.Rows[0]["EmailID"].ToString(), Password = dtFpt.Rows[0]["Password"].ToString();
                MailMessage mail = new MailMessage();//SmtpClient SmtpServer = new SmtpClient("mail.websmithsolution.com", 25);
                SmtpClient SmtpServer = new SmtpClient();
                //if (ConfigurationManager.AppSettings["EmailAddressFromForError"].ToString().Contains("websmithsolution"))
                //    SmtpServer.Host = "mail.websmithsolution.com";//SmtpServer.Port = 25;                
                //else if (ConfigurationManager.AppSettings["EmailAddressFromForError"].ToString().Contains("gmail"))
                //{
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;//SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                //}
                mail.From = new MailAddress(FromEmailID);
                mail.To.Add(ToEmailID);
                mail.Subject = " Device Log Messages.";
                string Body = "Deviece logs .<br />";
                Body += "<br />";
                Body += "Message : " + ex.Message + "<br />";
                Body += "Code Line : " + ex.StackTrace + " <br />";
                Body += "<br />";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpServer.Credentials = new System.Net.NetworkCredential(MailUserID, Password);
                SmtpServer.Send(mail);
                RetStr = "Email sent successfully.";
            }
            catch (Exception exs)
            {
                //  RetStr = exs.Message;
            }
            return RetStr;
        }
    }
}
