using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{
    [System.AttributeUsage(System.AttributeTargets.Class | AttributeTargets.Property)]
    public class TTAttributs : Attribute
    {
        private string Desc;
        public string FieldName;
        public SqlDbType ParamaterDataType;
        public bool isMemoField;
        public bool isInsertField;
        public bool isUpdateField;
        public bool isTableField;
        public bool isSelectField;

        public TTAttributs(string Description)
        {
            this.Desc = Description;
            FieldName = "Fieldname";
            ParamaterDataType = SqlDbType.NVarChar;
            isMemoField = false;
            isInsertField = true;
            isUpdateField = true;
            isTableField = true;
            isSelectField = true;
        }
    }
}
