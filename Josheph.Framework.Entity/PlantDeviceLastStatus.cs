using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data;
using System.Text;
using Josheph.Framework.Common;

namespace Josheph.Framework.Entity
{
    public class PlantDeviceLastStatus : TTCommonEntity
    {
        public PlantDeviceLastStatus()
        {
            this.TableName = "PlantDeviceLastStatus";
            this.CreatedDateTime = DateTime.Now;
            this.UpdatedDateTime = DateTime.Now;
            this.SystemDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("PlantDeviceLastStatus", FieldName = "laststatus_id", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "laststatus_id")]
        public System.Guid laststatus_id { get; set; }

        [TTAttributs("PlantDeviceLastStatus", FieldName = "laststatus_deviceid", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "laststatus_deviceid")]
        public System.Guid laststatus_deviceid { get; set; }

        [TTAttributs("PlantDeviceLastStatus", FieldName = "laststatus_status", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "laststatus_status")]
        public Decimal laststatus_status { get; set; }

        [TTAttributs("PlantDeviceLastStatus", FieldName = "laststatus_type", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "laststatus_type")]
        public int laststatus_type { get; set; }
    }
}
