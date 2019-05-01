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
    public class CountryMasterSUB : TTCommonEntity
    {
        public CountryMasterSUB()
        {
            this.TableName = "CountryMaster";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("CountryMaster", FieldName = "CountryID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "CountryID")]
        public System.Guid CountryID { get; set; }

        [TTAttributs("CountryMaster", FieldName = "CountryName", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "CountryName")]
        public string CountryName { get; set; }

        [TTAttributs("CountryMaster", FieldName = "Status", ParamaterDataType = SqlDbType.BigInt, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("CountryMaster", FieldName = "CreatedDate", ParamaterDataType = SqlDbType.VarChar, isUpdateField = false, isInsertField = false, isTableField = false, isSelectField = true, isMemoField = false)]
        [Display(Name = "CreatedDate")]
        public string CreatedDate { get; set; }

        //public string statustext
        //{
        //    get
        //    {
        //        string s = "NA";
        //        if (this.Status == MyEnumration.MyStatus.Active)
        //        {
        //            s = "Account is activated";
        //        }
        //        return s;
        //    }
        //}
    }
}
