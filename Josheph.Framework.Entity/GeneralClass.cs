using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Entity
{
    public static class GeneralClass
    {
        private static List<InverterStatus> lstInverterStatus = new List<InverterStatus>();

        private static void GenerateList()
        {
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "4", StatusMessage = "On Grid", InverterType = "Schneider" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "3", StatusMessage = "Irradiation Detecting", InverterType = "Schneider" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "1", StatusMessage = "No Irradiation", InverterType = "Schneider" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "7 2406", StatusMessage = "Shutdown: Abnormal Grid Voltage", InverterType = "Schneider" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "512", StatusMessage = "On Grid", InverterType = "Huawei" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "2", StatusMessage = "Irradiation Detecting", InverterType = "Huawei" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "40960", StatusMessage = "No Irradiation", InverterType = "Huawei" });
            lstInverterStatus.Add(new Entity.InverterStatus { StatusCode = "768", StatusMessage = "Shutdown: Abnormal Grid Voltage", InverterType = "Huawei" });
        }

        public static string GetStatus(string Type, string Status)
        {
            if (lstInverterStatus.Count == 0)
                GenerateList();
            var obj = lstInverterStatus.Where(x => x.InverterType == Type && x.StatusCode == Status).ToList();
            if (obj != null)
            {
                if (obj.Count > 0)
                {
                    return obj.First().StatusMessage;
                }
                else { return "Online"; }
            }
            else { return "Online"; }
        }
    }

    public class InverterStatus
    {
        public string InverterType { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}
