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
    public class OTPCodeMaster : TTCommonEntity
    {
        public OTPCodeMaster()
        {
            this.TableName = "OTPCodeMaster";
            this.CreatedDateTime = DateTime.Now;
            this.SystemDateTime = DateTime.Now;
            this.UpdatedDateTime = DateTime.Now;
        }

        [Key]
        [TTAttributs("OTPCodeMaster", FieldName = "otp_id", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "otp_id")]
        public System.Guid otp_id { get; set; }

        [TTAttributs("OTPCodeMaster", FieldName = "otp_user_id", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "otp_user_id")]
        public System.Guid otp_user_id { get; set; }
        
        [TTAttributs("OTPCodeMaster", FieldName = "otp_code", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "otp_code")]
        public int otp_code { get; set; }
        
    }
}
