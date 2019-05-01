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
    public class MainDeviceMasterSUB : TTCommonEntity
    {
        public MainDeviceMasterSUB()
        {
            this.TableName = "MainDeviceMaster";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("MainDeviceMaster", FieldName = "DeviceId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "DeviceId")]
        public System.Guid DeviceId { get; set; }

        [Key]
        [TTAttributs("MainDeviceMaster", FieldName = "PlantId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "PlantId")]
        public System.Guid PlantId { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "DeviceType", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "PlantName")]
        public int DeviceType { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "DeviceName", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "DeviceName")]
        [MaxLength(100)]
        public string DeviceName { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "SerialNo", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SerialNo")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "Make", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Make")]
        [MaxLength(100)]
        public string Make { get; set; }


        [TTAttributs("MainDeviceMaster", FieldName = "Location", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Location")]
        [MaxLength(100)]
        public string Location { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "InstallDate", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "InstallDate")]
        [MaxLength(50)]
        public string InstallDate { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "Address", ParamaterDataType = SqlDbType.NVarChar, isMemoField = true)]
        [Display(Name = "Address")]
        [MaxLength(100)]
        public string Address { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "IpAddress", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "IpAddress")]
        [MaxLength(50)]
        [RegularExpression(@"^(?:[0-9]{1,3}.){3}[0-9]{1,3}$")]
        public string IpAddress { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "Status", ParamaterDataType = SqlDbType.Int, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "PlantName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isSelectField = true, isTableField = false)]
        [Display(Name = "PlantName")]
        public string PlantName { get; set; }

        [TTAttributs("MainDeviceMaster", FieldName = "DeviceTypeName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isSelectField = true, isTableField = false)]
        [Display(Name = "DeviceTypeName")]
        public string DeviceTypeName { get; set; }

        
    }
}
