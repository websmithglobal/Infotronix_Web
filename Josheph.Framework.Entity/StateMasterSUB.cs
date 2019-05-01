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
    public class StateMasterSUB : TTCommonEntity
    {
        public StateMasterSUB()
        {
            this.TableName = "StateMaster";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("StateMaster", FieldName = "StateID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "StateID")]
        public System.Guid StateID { get; set; }

        [Key]
        [TTAttributs("StateMaster", FieldName = "CountryID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "CountryID")]
        public System.Guid CountryID { get; set; }      

        [TTAttributs("StateMaster", FieldName = "StateName", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "StateName")]
        public string StateName { get; set; }

        [TTAttributs("StateMaster", FieldName = "Status", ParamaterDataType = SqlDbType.BigInt, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("StateMaster", FieldName = "CreatedDate", ParamaterDataType = SqlDbType.VarChar, isUpdateField = false, isInsertField = false, isTableField = false, isSelectField = true, isMemoField = false)]
        [Display(Name = "CreatedDate")]
        public string CreatedDate { get; set; }
    }
}
