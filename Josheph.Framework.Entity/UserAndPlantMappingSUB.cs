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
    public class UserAndPlantMappingSUB : TTCommonEntity
    {
        public UserAndPlantMappingSUB()
        {
            this.TableName = "UserAndPlantMapping";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }

        [Key]
        [TTAttributs("UserAndPlantMapping", FieldName = "UserAndPlantMappingID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "UserAndPlantMappingID")]
        public System.Guid UserAndPlantMappingID { get; set; }

        [TTAttributs("UserAndPlantMapping", FieldName = "AspNetUserID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "AspNetUserID")]
        public System.Guid AspNetUserID { get; set; }

        [TTAttributs("UserAndPlantMapping", FieldName = "PlantId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "PlantId")]
        public System.Guid PlantId { get; set; }

        [TTAttributs("UserAndPlantMapping", FieldName = "DisplayName", ParamaterDataType = SqlDbType.UniqueIdentifier, isInsertField = false, isUpdateField = false, isSelectField = true, isTableField = false)]
        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }
    }
}
