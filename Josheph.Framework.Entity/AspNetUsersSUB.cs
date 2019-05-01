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
    public class AspNetUsersSUB : TTCommonEntity
    {
        public AspNetUsersSUB()
        {
            this.TableName = "AspNetUsers";
            this.CreatedDateTime = DateTime.Now;
            this.UpdatedDateTime = DateTime.Now;
            this.SystemDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("AspNetUsers", FieldName = "Id", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Id")]
        [MaxLength(128)]
        public string Id { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "Email", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Email")]
        [MaxLength(256)]
        public string Email { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "EmailConfirmed", ParamaterDataType = SqlDbType.Bit)]
        [Display(Name = "EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "PasswordHash", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "PasswordHash")]
        [MaxLength(-1)]
        public string PasswordHash { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "SecurityStamp", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "SecurityStamp")]
        [MaxLength(-1)]
        public string SecurityStamp { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "PhoneNumber", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "PhoneNumber")]
        [MaxLength(-1)]
        public string PhoneNumber { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "PhoneNumberConfirmed", ParamaterDataType = SqlDbType.Bit)]
        [Display(Name = "PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "TwoFactorEnabled", ParamaterDataType = SqlDbType.Bit)]
        [Display(Name = "TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "LockoutEndDateUtc", ParamaterDataType = SqlDbType.DateTime)]
        [Display(Name = "LockoutEndDateUtc")]
        public System.DateTime LockoutEndDateUtc { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "LockoutEnabled", ParamaterDataType = SqlDbType.Bit)]
        [Display(Name = "LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "AccessFailedCount", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "AccessFailedCount")]
        public int AccessFailedCount { get; set; }

        [TTAttributs("AspNetUsers", FieldName = "UserName", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "UserName")]
        [MaxLength(256)]
        public string UserName { get; set; }
    }
}
