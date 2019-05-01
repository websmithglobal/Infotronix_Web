using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL = Josheph.Framework.BusinessLayer;
using ENT = Josheph.Framework.Entity;

namespace Infotronix.PlantStatus
{
    public class CheckStatus
    {
        BAL.DeviceDataBAL objDashboard;
        BAL.SubDeviceMasterBAL objSubDevice;
        BAL.PlantDeviceLastStatus objLastStatus;
        private static List<ENT.PlantMasterSUB> lstPlant = new List<ENT.PlantMasterSUB>();
        private static List<ENT.SubDeviceMasterSUB> lstSubDevices = new List<ENT.SubDeviceMasterSUB>();

        public CheckStatus()
        {
            objDashboard = new BAL.DeviceDataBAL();
            objSubDevice = new Josheph.Framework.BusinessLayer.SubDeviceMasterBAL();
            lstPlant = new BAL.PlantMasterBAL().GetAll(string.Empty);
            lstSubDevices = objSubDevice.GetAll(string.Empty);
            objLastStatus = new Josheph.Framework.BusinessLayer.PlantDeviceLastStatus();
        }

        public void ReadStatus()
        {
            BAL.SmsManagement objBalSms = new Josheph.Framework.BusinessLayer.SmsManagement();
            // checking status of plant
            foreach (ENT.PlantMasterSUB el in lstPlant)
            {
                var intResult = objDashboard.GetPlantActiveMinutes(el.AspNetUserID);
                int ActiveMinutes = intResult.Sum(x => x.LastActMinutes);

                if (ActiveMinutes >= 30)
                {
                    List<ENT.PlantDeviceLastStatus> deviceStatus = objLastStatus.GetByDevice(el.PlantId, 1);
                    if (deviceStatus.Count > 0)
                    {
                        // 1 means online 2 means offline
                        if (deviceStatus[0].laststatus_status == 1)
                        {
                            String message = "ALERT -"+Environment.NewLine +el.PlantName + " Status changed to offline.";
                            Console.WriteLine(message);
                            // plant changed to offline send message
                            objBalSms.SendMessage("9426666404,8320399766", message);

                            // change plan status to offline
                            ENT.PlantDeviceLastStatus laststs = deviceStatus[0];
                            laststs.laststatus_status = 2;
                            objLastStatus.Update(laststs);
                        }
                    }
                    else
                    {
                        objLastStatus.Entity.laststatus_deviceid = el.PlantId;
                        objLastStatus.Entity.laststatus_type = 1;
                        objLastStatus.Entity.laststatus_status = 2;
                        objLastStatus.Insert(objLastStatus.Entity);
                        // plant changed to offline send message
                        String message = "ALERT -" + Environment.NewLine + el.PlantName + " Status changed to offline.";
                        Console.WriteLine(message);
                        objBalSms.SendMessage("9426666404,8320399766", message);
                    }
                }
                else
                {
                    List<ENT.PlantDeviceLastStatus> deviceStatus = objLastStatus.GetByDevice(el.PlantId, 1);
                    if (deviceStatus.Count > 0)
                    {
                        // 1 means online 2 means offline
                        if (deviceStatus[0].laststatus_status == 2)
                        {
                            // plant changed to Online send message
                            String message = "ALERT -" + Environment.NewLine + el.PlantName + " Status changed to is online.";
                            objBalSms.SendMessage("9426666404,8320399766", message);
                            Console.WriteLine(message);

                            ENT.PlantDeviceLastStatus laststs = deviceStatus[0];
                            laststs.laststatus_status = 1;
                            objLastStatus.Update(laststs);
                        }
                        // get all sub devices of plant and check status
                        CheckSubDeviceStatus(el);
                    }
                    else
                    {
                        objLastStatus.Entity.laststatus_deviceid = el.PlantId;
                        objLastStatus.Entity.laststatus_type = 1;
                        objLastStatus.Entity.laststatus_status = 1;
                        objLastStatus.Insert(objLastStatus.Entity);

                        String message = "NO STATUS FOR : " + el.PlantName;
                        Console.WriteLine(message);

                    }
                }
            }
        }

        public void CheckSubDeviceStatus(ENT.PlantMasterSUB Plant)
        {
            BAL.SmsManagement objBalSms = new Josheph.Framework.BusinessLayer.SmsManagement();

            List<ENT.SubDeviceMasterSUB> subDevices = new List<Josheph.Framework.Entity.SubDeviceMasterSUB>();
            subDevices = lstSubDevices.FindAll(x => x.PlantID == Plant.PlantId);

            foreach (ENT.SubDeviceMasterSUB el in subDevices)
            {
                var intResult = objSubDevice.GetLastStatus(el.SubDeviceId);

                if (intResult == 512 || intResult == 4 || intResult == 768 || intResult == 72406)
                {
                    List<ENT.PlantDeviceLastStatus> deviceStatus = objLastStatus.GetByDevice(el.SubDeviceId, 2);

                    if (deviceStatus.Count > 0)
                    {
                        if (deviceStatus[0].laststatus_status != intResult)
                        {
                            string status = string.Empty;

                            if (intResult == 512 || intResult == 4) { status = "On grid"; }
                            if (intResult == 2 || intResult == 3) { status = "Irradiation Detecting"; }
                            if (intResult == 40960 || intResult == 1) { status = "No Irradiation"; }
                            if (intResult == 768 || intResult == 72406) { status = "Shutdown: Abnormal Grid Voltage"; }

                            String message = "ALERT-"+Environment.NewLine+Plant.PlantName +Environment.NewLine+el.SubDeviceName+" STATUS = "+status;
                            
                            // sub device status changed send message
                            objBalSms.SendMessage("9426666404,8320399766", message);

                            // change plan status to offline
                            ENT.PlantDeviceLastStatus laststs = deviceStatus[0];
                            laststs.laststatus_status = intResult;
                            objLastStatus.Update(laststs);
                        }
                    }
                    else
                    {
                        objLastStatus.Entity.laststatus_deviceid = el.SubDeviceId;
                        objLastStatus.Entity.laststatus_type = 2;
                        objLastStatus.Entity.laststatus_status = intResult;
                        objLastStatus.Insert(objLastStatus.Entity);
                    }
                }
            }
        }
    }
}
