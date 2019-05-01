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
    public class SubDeviceMasterSUB : TTCommonEntity
    {
        public SubDeviceMasterSUB()
        {
            this.TableName = "SubDeviceMaster";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("SubDeviceMaster", FieldName = "SubDeviceId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "SubDeviceId")]
        public System.Guid SubDeviceId { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "PlantID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "PlantID")]
        public System.Guid PlantID { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "DeviceId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "DeviceId")]
        public System.Guid DeviceId { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "SubDeviceType", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "SubDeviceType")]
        public int SubDeviceType { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "SubDeviceName", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SubDeviceName")]
        [MaxLength(100)]
        public string SubDeviceName { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "SerialNo", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SerialNo")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "Make", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Make")]
        [MaxLength(100)]
        public string Make { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "Location", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Location")]
        [MaxLength(100)]
        public string Location { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "InstallDate", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "InstallDate")]
        [MaxLength(50)]
        public string InstallDate { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "Address", ParamaterDataType = SqlDbType.NVarChar, isMemoField = true)]
        [Display(Name = "Address")]
        [MaxLength(100)]
        public string Address { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "IpAddress", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "IpAddress")]
        [MaxLength(50)]
        public string IpAddress { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "FTPFolder", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "FTPFolder")]
        public string FTPFolder { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "FTPFilename", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "FTPFilename")]
        public string FTPFilename { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "FTPFileDateFormat", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "FTPFileDateFormat")]
        public string FTPFileDateFormat { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "Status", ParamaterDataType = SqlDbType.Int, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "SubDeviceTypeName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isSelectField = true, isTableField = false)]
        [Display(Name = "SubDeviceTypeName")]
        public string SubDeviceTypeName { get; set; }

        [TTAttributs("SubDeviceMaster", FieldName = "DeviceName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isSelectField = true, isTableField = false)]
        [Display(Name = "DeviceName")]
        public string DeviceName { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "MultiplyConversation", ParamaterDataType = SqlDbType.Real)]
        [Display(Name = "MultiplyConversation")]
        public decimal MultiplyConversation { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "PerformsOfPlantUniteVolume", ParamaterDataType = SqlDbType.Real)]
        [Display(Name = "PerformsOfPlantUniteVolume")]
        public decimal PerformsOfPlantUniteVolume { get; set; }
    }
}
