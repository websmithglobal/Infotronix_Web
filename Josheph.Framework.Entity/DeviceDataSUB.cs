using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Josheph.Framework.Common;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Josheph.Framework.Entity
{
    public class DeviceDataSUB : TTCommonEntity
    {
        public DeviceDataSUB()
        {
            this.TableName = "DeviceData";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }

        [Key]
        [TTAttributs("DeviceData", FieldName = "DeviceDataID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "DeviceDataID")]
        public System.Guid DeviceDataID { get; set; }

        [TTAttributs("DeviceData", FieldName = "SubDeviceId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "SubDeviceId")]
        public System.Guid SubDeviceId { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV1", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV1")]
        public decimal UPV1 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV2", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV2")]
        public decimal UPV2 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV3", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV3")]
        public decimal UPV3 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV4", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV4")]
        public decimal UPV4 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV5", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV5")]
        public decimal UPV5 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV6", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV6")]
        public decimal UPV6 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV7", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV7")]
        public decimal UPV7 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV8", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV8")]
        public decimal UPV8 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV9", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV9")]
        public decimal UPV9 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV10", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV9")]
        public decimal UPV10 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV11", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV11")]
        public decimal UPV11 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UPV12", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UPV12")]
        public decimal UPV12 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV1", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV1")]
        public decimal IPV1 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV2", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV2")]
        public decimal IPV2 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV3", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV3")]
        public decimal IPV3 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV4", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV4")]
        public decimal IPV4 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV5", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV5")]
        public decimal IPV5 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV6", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV6")]
        public decimal IPV6 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV7", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV7")]
        public decimal IPV7 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV8", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV8")]
        public decimal IPV8 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV9", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV9")]
        public decimal IPV9 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV10", ParamaterDataType = SqlDbType.Real)]
        [Display(Name = "IPV9")]
        public decimal IPV10 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV11", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV11")]
        public decimal IPV11 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IPV12", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IPV12")]
        public decimal IPV12 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UAC1", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UAC1")]
        public decimal UAC1 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UAC2", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UAC2")]
        public decimal UAC2 { get; set; }

        [TTAttributs("DeviceData", FieldName = "UAC3", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "UAC3")]
        public decimal UAC3 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IAC1", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IAC1")]
        public decimal IAC1 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IAC2", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IAC2")]
        public decimal IAC2 { get; set; }

        [TTAttributs("DeviceData", FieldName = "IAC3", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "IAC3")]
        public decimal IAC3 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Status", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Status")]
        [MaxLength(50)]
        public string Status { get; set; }

        [TTAttributs("DeviceData", FieldName = "Error", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Error")]
        public string Error { get; set; }

        [TTAttributs("DeviceData", FieldName = "Temp", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Temp")]
        public string Temp { get; set; }

        [TTAttributs("DeviceData", FieldName = "Cos", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Cos")]
        public decimal Cos { get; set; }

        [TTAttributs("DeviceData", FieldName = "Fac", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Fac")]
        public decimal Fac { get; set; }

        [TTAttributs("DeviceData", FieldName = "Pac", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Pac")]
        public decimal Pac { get; set; }

        [TTAttributs("DeviceData", FieldName = "Eac", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Eac")]
        public decimal Eac { get; set; }

        [TTAttributs("DeviceData", FieldName = "Qac", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Qac")]
        public decimal Qac { get; set; }

        [TTAttributs("DeviceData", FieldName = "DeviceDate", ParamaterDataType = SqlDbType.Date)]
        [Display(Name = "DeviceDate")]
        public string DeviceDate { get; set; }

        [TTAttributs("DeviceData", FieldName = "DeviceTime", ParamaterDataType = SqlDbType.Time)]
        [Display(Name = "DeviceTime")]
        public string DeviceTime { get; set; }

        [TTAttributs("DeviceData", FieldName = "DeviceDateTime", ParamaterDataType = SqlDbType.DateTime)]
        [Display(Name = "DeviceDateTime")]
        public DateTime DeviceDateTime { get; set; }

        [TTAttributs("DeviceData", FieldName = "DeviceDateTime", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "DeviceDateTime")]
        public string DeviceDateTimeText
        {
            get
            {
                return this.DeviceDateTime.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }

        [TTAttributs("DeviceData", FieldName = "TotalEnergy", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "TotalEnergy")]
        public decimal TotalEnergy { get; set; }

        [TTAttributs("DeviceData", FieldName = "TotalRealPower", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "TotalRealPower")]
        public decimal TotalRealPower { get; set; }

        [TTAttributs("DeviceData", FieldName = "Make", ParamaterDataType = SqlDbType.VarChar, isTableField = false, isInsertField = false, isUpdateField = false)]
        [Display(Name = "Make")]
        public string Make { get; set;}

        [TTAttributs("DeviceData", FieldName = "InvStatusText", ParamaterDataType = SqlDbType.VarChar, isTableField = false, isInsertField = false, isUpdateField = false)]
        [Display(Name = "InvStatusText")]
        public string InvStatusText
        {
            get
            {
                return GeneralClass.GetStatus(this.Make, this.Status);
            }
        }
    }
}
