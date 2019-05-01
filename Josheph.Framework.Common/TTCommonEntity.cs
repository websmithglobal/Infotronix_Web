using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{

    public class TTCommonEntity
    {
        [TTAttributs("CommonFields", FieldName = "SystemDateTime", ParamaterDataType = SqlDbType.DateTime, isUpdateField = false)]
        [Description("this field will update system date time when ever user insert row to table.")]
        public DateTime SystemDateTime { get; set; }

        [TTAttributs("CommonFields", FieldName = "CreatedBy", ParamaterDataType = SqlDbType.UniqueIdentifier, isUpdateField = false)]
        [Description("Created by user master table primary key")]
        public Guid CreatedBy { get; set; }

        [TTAttributs("CommonFields", FieldName = "CreatedByName", ParamaterDataType = SqlDbType.NVarChar, isUpdateField = false, isTableField = false, isInsertField = false, isSelectField = true)]
        [Description("Created by user master table primary key")]
        public string CreatedByName { get; set; }

        [TTAttributs("CommonFields", FieldName = "CreatedDateTime", ParamaterDataType = SqlDbType.DateTime, isUpdateField = false)]
        [Description("This field have created datetime when user crate row")]
        public DateTime CreatedDateTime { get; set; }

        [TTAttributs("CommonFields", FieldName = "UpdatedBy", ParamaterDataType = SqlDbType.UniqueIdentifier, isInsertField = false)]
        [Description("Update by user master table primary key")]
        public Guid UpdatedBy { get; set; }

        [TTAttributs("CommonFields", FieldName = "UpdatedByName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false, isSelectField = true)]
        [Description("Update by user master table primary key")]
        public string UpdatedByName { get; set; }

        [TTAttributs("CommonFields", FieldName = "UpdatedDateTime", ParamaterDataType = SqlDbType.DateTime, isInsertField = false)]
        [Description("This field have updated datetime when user update row")]
        public DateTime UpdatedDateTime { get; set; }

        [TTAttributs("TableName", FieldName = "TableName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false, isSelectField = false)]
        [Description("This Property has table name which is belong to entity")]
        public String TableName { get; set; }

        [TTAttributs("TableName", FieldName = "SystemDateTimeFormat", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false, isSelectField = false)]
        [Description("This Property has table name which is belong to entity")]
        public String SystemDateTimeFormat
        {
            get
            {
                return this.SystemDateTime.GetFormatedDateTime();
            }
        }

        [TTAttributs("TableName", FieldName = "EntryMode", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false, isSelectField = false)]
        [Description("this field whill have entry mode true when user want insert record to table")]
        public MyEnumration.EntryMode EntryMode { get; set; }

        public Dictionary<string, bool> FieldCollection;
    }
    
    public static class TTPagination
    {
        [DefaultValue(1)]
        public static long PageNo { get; set; }
        [DefaultValue(10)]
        public static long PageSize { get; set; }

        [DefaultValue(0)]
        public static long RecordCount { get; set; }

        [DefaultValue(false)]
        public static bool isPageing { get; set; }

        [DefaultValue(0)]
        public static long PageCount { get; set; }
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }
}
