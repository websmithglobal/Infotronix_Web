using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{
    public static class CommonMSSQL
    {
        #region SQL Command Operations
        public static Boolean AddParameter(SqlCommand parCMD, object obj, string parPropertyName, bool isInsertField)
        {
            Boolean blnResult = false;
            //TTAttributs MyAttributs = (TTAttributs)obj.GetType().GetProperty(parPropertyName).GetCustomAttribute(typeof(TTAttributs), false);
            TTAttributs MyAttributs = HelperMethod.GetTTAttributes(obj, parPropertyName);
            object objValue = HelperMethod.GetPropValue(obj, parPropertyName);

            if (ParOperationValidation(MyAttributs, isInsertField))
            {
                if (MyAttributs.ParamaterDataType == SqlDbType.NVarChar)
                {
                    int parFieldWidth = 50;
                    if (MyAttributs.isMemoField == false)
                    {
                        parFieldWidth = ((MaxLengthAttribute)(obj.GetType().GetProperty(parPropertyName).GetCustomAttributes(typeof(MaxLengthAttribute), true)[0])).Length;
                        parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType, parFieldWidth).Value = objValue == null ? string.Empty : objValue;
                    }
                    else
                    {
                        parFieldWidth = objValue == null ? 0 : objValue.ToString().Length;
                        parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType, parFieldWidth).Value = objValue == null ? string.Empty : objValue;
                    }
                }
                else
                {
                    parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType).Value = objValue == null ? string.Empty : objValue;
                }
            }
            return blnResult;
        }
        public static Boolean AddParameter(SqlCommand parCMD, object obj, string parPropertyName)
        {
            Boolean blnResult = false;
            //TTAttributs MyAttributs = (TTAttributs)obj.GetType().GetProperty(parPropertyName).GetCustomAttribute(typeof(TTAttributs), false);
            TTAttributs MyAttributs = HelperMethod.GetTTAttributes(obj, parPropertyName);
            object objValue = HelperMethod.GetPropValue(obj, parPropertyName);

            if (MyAttributs.ParamaterDataType == SqlDbType.NVarChar)
            {
                int parFieldWidth = 50;
                if (MyAttributs.isMemoField == false)
                {
                    parFieldWidth = ((MaxLengthAttribute)(obj.GetType().GetProperty(parPropertyName).GetCustomAttributes(typeof(MaxLengthAttribute), true)[0])).Length;
                    parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType, parFieldWidth).Value = objValue == null ? string.Empty : objValue;
                }
                else
                {
                    parFieldWidth = objValue == null ? 0 : objValue.ToString().Length;
                    parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType, parFieldWidth).Value = objValue == null ? string.Empty : objValue;
                }
            }
            else
            {
                parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType).Value = objValue;
            }
            return blnResult;
        }
        public static Boolean AddParameter(SqlCommand parCMD, TTDictionary lstParamters, object objEntity)
        {
            Boolean blnResult = false;
            //TTAttributs MyAttributs = (TTAttributs)obj.GetType().GetProperty(parPropertyName).GetCustomAttribute(typeof(TTAttributs), false);

            for (int i = 0; i < lstParamters.Count(); i++)
            {
                var el = lstParamters.ElementAt(i);
                TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, el.Key);
                object objValue = el.Value.Value1;
                if (MyAttributs.ParamaterDataType == SqlDbType.NVarChar)
                {
                    int parFieldWidth = ((MaxLengthAttribute)(objEntity.GetType().GetProperty(el.Key).GetCustomAttributes(typeof(MaxLengthAttribute), true)[0])).Length;
                    if (MyAttributs.isMemoField == false)
                    {
                        parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType, parFieldWidth).Value = objValue == null ? string.Empty : objValue;
                    }
                    else
                    {
                        parFieldWidth = objEntity.ToString().Length;
                        parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType, parFieldWidth).Value = objValue == null ? string.Empty : objValue;
                    }
                }
                else
                {
                    parCMD.Parameters.Add("@" + MyAttributs.FieldName, MyAttributs.ParamaterDataType).Value = objValue;
                }
            }
            return blnResult;
        }
        private static bool ParOperationValidation(TTAttributs parAttributs, bool parInsert)
        {
            if (parInsert)
            {
                return parAttributs.isInsertField;
            }
            else
            {
                return parAttributs.isUpdateField;
            }
        }
        public static void ClearParameter(SqlCommand parCMD)
        {
            parCMD.Parameters.Clear();
        }

        #endregion

        #region Generate Query from object
        public static String PrepairInsertQuery(SqlCommand parCMD, string parTableName)
        {
            string strQueryResult = "";

            string strFields = ""; ;
            string strValues = "";

            for (int i = 0; i < parCMD.Parameters.Count; i++)
            {
                if (i != parCMD.Parameters.Count - 1)
                {
                    strFields += parCMD.Parameters[i].ParameterName.Replace("@", "") + ",";
                    strValues += parCMD.Parameters[i].ParameterName + ",";

                }
                else
                {
                    strFields += parCMD.Parameters[i].ParameterName.Replace("@", "");
                    strValues += parCMD.Parameters[i].ParameterName;
                }
            }
            strFields = "Insert Into " + parTableName + " ( " + strFields + " ) Values";
            strValues = "(" + strValues + ")";
            strQueryResult = strFields + strValues;
            return strQueryResult;
        }
        public static String PrepairUpdateQuery(SqlCommand parCMD, string parTableName, object objPrimaryKey)
        {
            string strQueryResult = "";
            string strFields = ""; ;
            string strParameter = "";
            string strPrimaryKey = "";

            #region Get Primarykey Information from object
            PropertyInfo[] properties = objPrimaryKey.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;
                if (attribute != null) // This property has a KeyAttribute
                {
                    // Do something, to read from the property:
                    //object val = property.GetValue(objPrimaryKey);
                    strPrimaryKey = ((TTAttributs)objPrimaryKey.GetType().GetProperty(property.Name).GetCustomAttribute(typeof(TTAttributs), false)).FieldName;
                    break;
                }
            }

            #endregion


            for (int i = 0; i < parCMD.Parameters.Count; i++)
            {
                if (!parCMD.Parameters[i].ParameterName.Contains(strPrimaryKey))
                {
                    if (i != parCMD.Parameters.Count - 1)
                    {
                        strFields += parCMD.Parameters[i].ParameterName.Replace("@", "") + "=" + parCMD.Parameters[i].ParameterName + ",";
                    }
                    else
                    {
                        strFields += parCMD.Parameters[i].ParameterName.Replace("@", "") + "=" + parCMD.Parameters[i].ParameterName;
                    }
                }
                else
                {
                    strParameter = parCMD.Parameters[i].ParameterName.Replace("@", "") + "=" + parCMD.Parameters[i].ParameterName;
                }
            }
            strFields = "Update " + parTableName + " Set  " + strFields;
            strParameter = " Where " + strParameter;
            strQueryResult = strFields + strParameter;
            return strQueryResult;
        }
        public static String PrepairPartialUpdateQuery(Dictionary<string, bool> parFields, string parTableName, object objEntity)
        {
            string strQueryResult = "";
            string strFields = ""; ;
            string strParameter = "";
            Dictionary<string, bool> flds = parFields.Where(x => x.Value == false).ToDictionary(x => x.Key, x => x.Value);

            if (flds.Count > 1)
            {
                for (int i = 0; i < flds.Count; i++)
                {
                    var element = flds.ElementAt(i);
                    TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, element.Key);
                    if (i != flds.Count - 1)
                    {
                        strFields += MyAttributs.FieldName + "= @" + MyAttributs.FieldName + ",";
                    }
                    else
                    {
                        strFields += MyAttributs.FieldName + "= @" + MyAttributs.FieldName;
                    }
                }
            }
            else
            {
                var element = flds.ElementAt(0);
                TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, element.Key);
                strFields = MyAttributs.FieldName + "= @" + MyAttributs.FieldName;
            }

            Dictionary<string, bool> parm = parFields.Where(x => x.Value == true).ToDictionary(x => x.Key, x => x.Value);
            if (parm.Count > 1)
            {
                for (int i = 0; i < parm.Count; i++)
                {
                    var element = parm.ElementAt(i);
                    TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, element.Key);
                    if (element.Value == true)
                    {
                        if (i != parm.Count - 1)
                        {
                            strParameter += MyAttributs.FieldName + "= @" + MyAttributs.FieldName + " AND ";
                        }
                        else
                        {
                            strParameter += MyAttributs.FieldName + "= @" + MyAttributs.FieldName;
                        }
                    }
                }
            }
            else
            {
                var element = parm.ElementAt(0);
                TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, element.Key);
                strParameter = MyAttributs.FieldName + "= @" + MyAttributs.FieldName;
            }

            strFields = "Update " + parTableName + " Set  " + strFields;
            strParameter = " Where " + strParameter;
            strQueryResult = strFields + strParameter;
            return strQueryResult;
        }
        public static String PrepairPartialDeleteQuery(Dictionary<string, bool> parFields, string parTableName, object objEntity)
        {
            string strQueryResult = "";
            string strFields = ""; ;
            string strParameter = "";

            Dictionary<string, bool> parm = parFields.Where(x => x.Value == true).ToDictionary(x => x.Key, x => x.Value);
            if (parm.Count > 1)
            {
                for (int i = 0; i < parm.Count; i++)
                {
                    var element = parm.ElementAt(i);
                    TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, element.Key);
                    if (element.Value == true)
                    {
                        if (i != parm.Count - 1)
                        {
                            strParameter += MyAttributs.FieldName + "= @" + MyAttributs.FieldName + " AND ";
                        }
                        else
                        {
                            strParameter += MyAttributs.FieldName + "= @" + MyAttributs.FieldName;
                        }
                    }
                }
            }
            else
            {
                var element = parm.ElementAt(0);
                TTAttributs MyAttributs = HelperMethod.GetTTAttributes(objEntity, element.Key);
                strParameter = MyAttributs.FieldName + "= @" + MyAttributs.FieldName;
            }

            strFields = "Delete " + parTableName;
            strParameter = " Where " + strParameter;
            strQueryResult = strFields + strParameter;
            return strQueryResult;
        }
        public static String PrepairDeleteQuery(string parTableName, object objPrimaryKey)
        {
            string strQueryResult = "";
            string strPrimaryKey = "";
            #region Get Primarykey Information from object
            PropertyInfo[] properties = objPrimaryKey.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;
                if (attribute != null) // This property has a KeyAttribute
                {
                    strPrimaryKey = ((TTAttributs)objPrimaryKey.GetType().GetProperty(property.Name).GetCustomAttribute(typeof(TTAttributs), false)).FieldName;
                    break;
                }
            }

            #endregion
            strQueryResult = String.Format("Delete {0} Where {1} = @{2}", parTableName, strPrimaryKey, strPrimaryKey);
            return strQueryResult;
        }
        public static String PrepairSelectPrimaryKey(string parTableName, object objEntity)
        {
            string strQueryResult = "";
            string strFields = "";
            string strParameter = "";
            string strPrimaryKey = "";

            #region Get Primarykey Information from object
            PropertyInfo[] properties = objEntity.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;
                if (attribute != null) // This property has a KeyAttribute
                {
                    // Do something, to read from the property:
                    //object val = property.GetValue(objPrimaryKey);
                    strPrimaryKey = ((TTAttributs)objEntity.GetType().GetProperty(property.Name).GetCustomAttribute(typeof(TTAttributs), false)).FieldName;
                    break;
                }
            }

            #endregion

            properties = objEntity.GetType().GetProperties();
            List<string> fieldlist = new List<string>();
            for (int i = 0; i < properties.Count(); i++)
            {
                var el = properties.ElementAt(i);
                if (el != null)
                {
                    TTAttributs tt = HelperMethod.GetTTAttributes(objEntity, el.Name);
                    if (tt.isTableField == true)
                    {
                        fieldlist.Add(tt.FieldName);
                    }
                }

            }
            strFields = string.Join(",", fieldlist);
            strParameter = strPrimaryKey + "= @" + strPrimaryKey;

            strFields = "Select " + strFields + " From " + parTableName;
            strParameter = " Where " + strParameter;
            strQueryResult = strFields + strParameter;
            return strQueryResult;
        }
        public static String PrepairSelectPerameters(Dictionary<string, MyEnumration.Operation> parFields, string parTableName, object objEntity)
        {
            string strQueryResult = "";
            string strFields = "";
            string strParameter = "";

            List<string> fieldlist = new List<string>();
            List<string> lstParam = new List<string>();

            PropertyInfo[] properties = objEntity.GetType().GetProperties();

            for (int i = 0; i < properties.Count(); i++)
            {
                var el = properties.ElementAt(i);
                if (el != null)
                {
                    TTAttributs tt = HelperMethod.GetTTAttributes(objEntity, el.Name);
                    if (tt.isTableField == true)
                    {
                        fieldlist.Add(tt.FieldName);
                    }
                }

            }

            if (parFields.Count > 0)
            {

                for (int i = 0; i < parFields.Count; i++)
                {
                    var el = parFields.ElementAt(i);
                    lstParam.Add(el.Value.ToString() + " " + el.Key + "=@" + el.Key);
                }
            }

            strFields = string.Join(",", fieldlist);
            strParameter = string.Join(" ", lstParam);
            strFields = "Select " + strFields + " From " + parTableName + " ";
            strQueryResult = strFields + strParameter;
            return strQueryResult;
        }

        #endregion
    }
}
