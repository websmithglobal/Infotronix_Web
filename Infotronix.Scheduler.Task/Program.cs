using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL = Josheph.Framework.BusinessLayer;
using ENT = Josheph.Framework.Entity;

namespace Infotronix.Scheduler.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args[0] == "sendmail")
            {
                //Console.WriteLine("MailSending Intialized ....");
                //using (BAL.DeviceDataBAL obj = new BAL.DeviceDataBAL())
                //{
                //    Console.WriteLine("Getting Energy Data....");
                //    DateTime dttm = DateTime.Now;
                //    dttm = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //    obj.GetDailyEnergyService(dttm, dttm);
                //    Console.WriteLine("Getting Energy Data Completed....");
                //}
                BAL.SMTPManagement objSendMail = new BAL.SMTPManagement();
                objSendMail.SendPlantDailyMail();
            }
        }
    }
}
