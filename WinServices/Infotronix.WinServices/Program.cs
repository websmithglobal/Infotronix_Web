using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BAL = Josheph.Framework.BusinessLayer;

namespace Infotronix.WinServices
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                BAL.SMTPManagement objSendMail = new BAL.SMTPManagement();
                objSendMail.SendPlantDailyMail();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new srvReadInverterData()
                };
                ServiceBase.Run(ServicesToRun);

            }

        }
    }
}
