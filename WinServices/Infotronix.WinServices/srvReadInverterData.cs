using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BAL = Josheph.Framework.BusinessLayer;
using ENT = Josheph.Framework.Entity;


namespace Infotronix.WinServices
{
    public partial class srvReadInverterData : ServiceBase
    {
        System.Timers.Timer objTime = new System.Timers.Timer();
        //System.Timers.Timer objSecondTick = new System.Timers.Timer();

        private System.Diagnostics.EventLog eventLog1;

        public srvReadInverterData()
        {
            InitializeComponent();
            this.eventLog1 = new System.Diagnostics.EventLog();

            //if (!EventLog.SourceExists("WebSmithSolarServices"))
            //    EventLog.CreateEventSource("WebSmithSolarServices", "WebSmithInverterReading");


            this.ServiceName = "Spark99AmbitSolarServices";
            this.EventLog.Log = "Spark99AmbitDataReading";


            // These Flags set whether or not to handle that specific
            //  type of event. Set to true if you need it, false otherwise.
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;

            //((ISupportInitialize)(this.EventLog)).BeginInit();
            //if (!EventLog.SourceExists(this.EventLog.Source))
            //{
            //    EventLog.CreateEventSource(this.EventLog.Source, this.EventLog.Log);
            //}
            //((ISupportInitialize)(this.EventLog)).EndInit();

            this.ServiceName = "Spark99AmbitSolarServices";
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";

            //this.EventLog.WriteEntry("Services Initilized Successfully." + DateTime.Now.ToString());
        }

        protected override void OnStart(string[] args)
        {
            this.EventLog.WriteEntry("Services Started Successfully." + DateTime.Now.ToString());
            objTime = new System.Timers.Timer();
            objTime.Elapsed += new System.Timers.ElapsedEventHandler(ObjTime_Elapsed);
            objTime.Interval = ((1000 * 60) * 2);
            objTime.Enabled = true;
            objTime.AutoReset = true;
            objTime.Start();
            //objSecondTick = new System.Timers.Timer();
            //objSecondTick.Elapsed += ObjSecondTick_Elapsed;
            //objSecondTick.Interval = ((1000 * 60) * 1);
            //objSecondTick.Enabled = true;
            //objSecondTick.AutoReset = true;
            //objSecondTick.Start();
            this.EventLog.WriteEntry("Timer Started Successfully." + DateTime.Now.ToString());
        }

        protected override void OnStop()
        {
            objTime.Enabled = false;
            objTime.AutoReset = false;
            objTime.Stop();

            //objSecondTick.Enabled = false;
            //objSecondTick.AutoReset = false;
            //objSecondTick.Stop();

            this.EventLog.WriteEntry("Services Stopped Successfully." + DateTime.Now.ToString());
        }

        private void ObjTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.EventLog.WriteEntry("Tick Started " + DateTime.Now.ToString());
            try
            {
                using (ReadDataFTP obj = new ReadDataFTP())
                {
                    this.EventLog.WriteEntry("Data Read Started Successfully." + DateTime.Now.ToString());
                    //objTime.Stop();
                    obj.ReadData();
                    this.EventLog.WriteEntry("Data Read Successfully." + DateTime.Now.ToString());
                }
                objTime.Start();
            }
            catch (Exception ex)
            {
                objTime.Start();
                this.EventLog.WriteEntry("Error: " + ex.Message + " " + DateTime.Now.ToString());
            }
        }

        //private void ObjSecondTick_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        TimeSpan span = DateTime.Now.TimeOfDay;
        //        if (span.Hours == 23 && span.Minutes == 0)
        //        {
        //            objSecondTick.Stop();
        //            this.EventLog.WriteEntry("Data Generation and Mail Sending Started Successfully." + DateTime.Now.ToString());
        //            using (BAL.DeviceDataBAL obj = new BAL.DeviceDataBAL())
        //            {
        //                DateTime dttm = DateTime.Now;
        //                dttm = DateTime.ParseExact(DateTime.Now.ToString("dd/mm/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                obj.GetDailyEnergyService(dttm, dttm);
        //            }
        //            BAL.SMTPManagement objSendMail = new BAL.SMTPManagement();
        //            objSendMail.SendPlantDailyMail();
        //            this.EventLog.WriteEntry("Data Generation and Mail Sent Successfully." + DateTime.Now.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.EventLog.WriteEntry("Error: " + ex.Message + " " + DateTime.Now.ToString());
        //    }
        //    finally
        //    {
        //        objSecondTick.Start();
        //        this.EventLog.WriteEntry("Data Generation Timer Started Successfully." + DateTime.Now.ToString());
        //    }
        //}

    }
}
