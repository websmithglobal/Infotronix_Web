using Josheph.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Entity
{
    public class MessageSendLog : TTCommonEntity
    {
        public MessageSendLog()
        {
            this.TableName = "MessageSendLog";
            this.CreatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("MessageSendLog", FieldName = "MessageLogId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "MessageLogId")]
        public System.Guid MessageLogId { get; set; }

        [TTAttributs("MessageSendLog", FieldName = "Mobile", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Mobile")]
        [MaxLength(50)]
        public string Mobile { get; set; }

        [TTAttributs("MessageSendLog", FieldName = "Response", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Response")]
        [MaxLength(-1)]
        public string Response { get; set; }

        [TTAttributs("MessageSendLog", FieldName = "IsSent", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "IsSent")]
        public int IsSent { get; set; }
    }
}
