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
    public class CityMasterSUB : TTCommonEntity
    {
        public CityMasterSUB()
        {
            this.TableName = "CityMaster";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("CityMaster", FieldName = "CityID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "CityID")]
        public System.Guid CityID { get; set; }

        //[Key]
        //[TTAttributs("CityMaster", FieldName = "CountryID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        //[Display(Name = "CountryID")]
        //public System.Guid CountryID { get; set; }

        [Key]
        [TTAttributs("CityMaster", FieldName = "StateID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "StateID")]
        public System.Guid StateID { get; set; }

        [TTAttributs("CityMaster", FieldName = "CityName", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "CityName")]
        public string CityName { get; set; }

        [TTAttributs("CityMaster", FieldName = "Status", ParamaterDataType = SqlDbType.BigInt, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("CityMaster", FieldName = "CreatedDate", ParamaterDataType = SqlDbType.VarChar, isUpdateField = false, isInsertField = false, isTableField = false, isSelectField = true, isMemoField = false)]
        [Display(Name = "CreatedDate")]
        public string CreatedDate { get; set; }
    }
}
