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
    public class AdminMasterSUB : TTCommonEntity
    {

        public AdminMasterSUB()
        {
            this.TableName = "AdminMaster";
            this.CreatedDateTime = this.SystemDateTime = this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("AdminMaster", FieldName = "AdminID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "AdminID")]
        public System.Guid AdminID { get; set; }

        [TTAttributs("AdminMaster", FieldName = "DisplayName", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [TTAttributs("AdminMaster", FieldName = "Email", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [TTAttributs("AdminMaster", FieldName = "UserRole", ParamaterDataType = SqlDbType.VarChar)]
        [Display(Name = "UserRole")]
        public string UserRole { get; set; }


        [TTAttributs("AdminMaster", FieldName = "Password", ParamaterDataType = SqlDbType.NVarChar, isTableField =false, isSelectField = false)]
        [Display(Name = "Password")]
        [MaxLength(50)]
        public string Password { get; set; }

        [TTAttributs("AdminMaster", FieldName = "Status", ParamaterDataType = SqlDbType.Int, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("", FieldName = "CreatedDateTimeText", ParamaterDataType = SqlDbType.NVarChar, isTableField = false, isSelectField = true,isInsertField = false,isUpdateField = false)]
        [Display(Name = "CreatedDateTimeText")]
        [MaxLength(50)]
        public string CreatedDateTimeText { get; set; }

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
