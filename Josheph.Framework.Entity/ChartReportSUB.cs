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
    class ChartReportSUB
    {
    }

    public class LastActivityMinutes
    {
        [TTAttributs("DeviceData", FieldName = "LastActMinutes", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "LastActMinutes")]
        public int LastActMinutes { get; set; }

        [TTAttributs("DeviceData", FieldName = "LastDateTime", ParamaterDataType = SqlDbType.DateTime)]
        [Display(Name = "LastDateTime")]
        public DateTime LastDateTime { get; set; }
    }

    public class DashboardCards
    {
        [TTAttributs("DeviceData", FieldName = "SerialNo", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SerialNo")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        [TTAttributs("DeviceData", FieldName = "EAC", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "EAC")]
        public decimal EAC { get; set; }

        [TTAttributs("DeviceData", FieldName = "EACString", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "EACString")]
        [MaxLength(50)]
        public string EACString { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "PerformsOfPlantUniteVolume", ParamaterDataType = SqlDbType.Real)]
        [Display(Name = "PerformsOfPlantUniteVolume")]
        public decimal PerformsOfPlantUniteVolume { get; set; }
    }

    public class DashboardCardsNew
    {
        [TTAttributs("DeviceData", FieldName = "SerialNo", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SerialNo")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        [TTAttributs("DeviceData", FieldName = "EAC", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "EAC")]
        public string EAC { get; set; }

        [TTAttributs("DeviceData", FieldName = "EACString", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "EACString")]
        public string EACString { get; set; }
    }


    public class DailyBallChartEntry : TTCommonEntity
    {
        public DailyBallChartEntry()
        {
            this.TableName = "DailyBallAllChart";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }

        [TTAttributs("DailyBallAllChart", FieldName = "DailyBallAllChartID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "DailyBallAllChartID")]
        [MaxLength(50)]
        public Guid DailyBallAllChartID { get; set; }

        [TTAttributs("DailyBallAllChart", FieldName = "AspNetUserID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "AspNetUserID")]
        [MaxLength(50)]
        public Guid AspNetUserID { get; set; }


        [TTAttributs("DailyBallAllChart", FieldName = "ReportDate", ParamaterDataType = SqlDbType.DateTime)]
        [Display(Name = "ReportDate")]
        public DateTime ReportDate { get; set; }

        [TTAttributs("DailyBallAllChart", FieldName = "DispalyData", ParamaterDataType = SqlDbType.Text, isMemoField = true)]
        [Display(Name = "DispalyData")]
        public string DispalyData { get; set; }

        [TTAttributs("DailyBallAllChart", FieldName = "SubDeviceID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "SubDeviceID")]
        public Guid SubDeviceID { get; set; }

        [TTAttributs("DailyBallAllChart", FieldName = "SubDeviceName", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SubDeviceName")]
        [MaxLength(200)]
        public string SubDeviceName { get; set; }

        [TTAttributs("DailyBallAllChart", FieldName = "IsConverted", ParamaterDataType = SqlDbType.Bit)]
        [Display(Name = "IsConverted")]
        public bool IsConverted { get; set; }
    }

    public class InverterDateTable
    {
        [TTAttributs("DeviceData", FieldName = "SerialNo", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SerialNo")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        [TTAttributs("DeviceData", FieldName = "DeviceName", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "DeviceName")]
        [MaxLength(50)]
        public string DeviceName { get; set; }

        [TTAttributs("DeviceData", FieldName = "PerformsOfPlantUniteVolume", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "PerformsOfPlantUniteVolume")]
        public decimal PerformsOfPlantUniteVolume { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day1", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day1")]
        public decimal Day1 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day2", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day2")]
        public decimal Day2 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day3", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day3")]
        public decimal Day3 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day4", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day4")]
        public decimal Day4 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day5", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day5")]
        public decimal Day5 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day6", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day6")]
        public decimal Day6 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Day7", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Day7")]
        public decimal Day7 { get; set; }

        [TTAttributs("DeviceData", FieldName = "Total7Days", ParamaterDataType = SqlDbType.Decimal)]
        [Display(Name = "Total7Days")]
        public decimal Total7Days { get; set; }

        [TTAttributs("DeviceData", FieldName = "InvStatus", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "InvStatus")]
        public string InvStatus { get; set; }

        [TTAttributs("DeviceData", FieldName = "InvStatus", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "InvStatus")]
        public string InvStatusText
        {
            get
            {
                return GeneralClass.GetStatus(this.Make, this.InvStatus);
            }
        }

        [TTAttributs("DeviceData", FieldName = "Make", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "Make")]
        public string Make { get; set; }
    }

    public class BarChartClass
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }

    public class BarAreaChartClass
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }

    public class LineChartClass
    {
        public string label { get; set; }
        public string value { get; set; }
    }
}
