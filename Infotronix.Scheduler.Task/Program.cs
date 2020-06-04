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
            try
            {
                new PlantEmailLogic().SendPlantEmail();
            }
            catch (Exception ee)
            {
                Console.WriteLine("ERROR in Main: " + ee.Message);
            }

            //BAL.SMTPManagement objSendMail = new BAL.SMTPManagement();

            //objSendMail.SendPlantDailyMail();
        }
    }
}