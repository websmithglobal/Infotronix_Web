using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{
    public static class HelperMethod
    {
        #region Entity Object Helper Methods
        public static string GetTableName(object Entity)
        {
            return GetPropValue(Entity, "TableName").ToString();
        }
        public static string GetPropertyName(object obj)
        {
            string strName = "";
            foreach (var a in obj.GetType().GetProperties())
            {
                strName = a.Name;
            }

            return strName;
        }
        //this static function for get property name from entity class
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public static TTAttributs GetTTAttributes(object obj, string strpropname)
        {
            return (TTAttributs)obj.GetType().GetProperty(strpropname).GetCustomAttribute(typeof(TTAttributs), false);
        }
        public static string PropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            //COM.CommonMSSQL.AddParameter(sqlCMD, Entity, PropertyName<ENT.ContactUS>(x => x.CustomerName));
            return body.Member.Name;
        }
        public static object SetValueInObject(object obj, object objValue, string parPropartyName)
        {
            obj.GetType().GetProperty(parPropartyName).SetValue(obj, objValue, null);
            return obj;
        }
        public static StringBuilder GetParameter(TTDictionary FieldCollection, StringBuilder sb)
        {
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < FieldCollection.Count(); i++)
            {
                var el = FieldCollection.ElementAt(i);
                sbResult.Append(" " + el.Value.Value2 + " " + el.Key + " " + el.Value.value3 + " @" + el.Key);
            }
            return sbResult;
        }

        public static string GetPageingString(string sbQuery, object objEntity)
        {
            string strResult = "";

            #region Get Primarykey Information from object
            PropertyInfo[] properties = objEntity.GetType().GetProperties();
            string strPrimaryKey = "";
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

            if (string.IsNullOrWhiteSpace(sbQuery))
            {
                //if (sbQuery.ToString().ToUpper().Contains(strPrimaryKey.ToUpper().Trim()))
                //{
                //    strResult = "," + strPrimaryKey + "ASC OFFSET ((" + TTPagination.PageNo + " - 1) * " + TTPagination.PageSize + ") ROWS FETCH NEXT " + TTPagination.PageSize + " ROWS ONLY";
                //}
                //else {
                //    strResult = " OFFSET ((" + TTPagination.PageNo + " - 1) * " + TTPagination.PageSize + ") ROWS FETCH NEXT " + TTPagination.PageSize + " ROWS ONLY";
                //}

                strResult = " ORDER BY " + strPrimaryKey + " ASC OFFSET ((" + TTPagination.PageNo + " - 1) * " + TTPagination.PageSize + ") ROWS FETCH NEXT " + TTPagination.PageSize + " ROWS ONLY";
            }
            else
            {
                strResult = " OFFSET ((" + TTPagination.PageNo + " - 1) * " + TTPagination.PageSize + ") ROWS FETCH NEXT " + TTPagination.PageSize + " ROWS ONLY";
            }
            return strResult;
        }

        #endregion

        #region Extension methods
        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            List<Variance> variances = new List<Variance>();
            PropertyInfo[] fi = val1.GetType().GetProperties();
            foreach (PropertyInfo f in fi)
            {
                Variance v = new Variance();
                v.Prop = f.Name;
                if (!v.Prop.ContainsAny("EntryMode", "SystemDateTime", "CreatedBy", "CreatedDateTime", "UpdatedBy", "UpdatedDateTime", "TableName", "EntryMode"))
                {
                    v.valA = f.GetValue(val1);
                    v.valB = f.GetValue(val2);
                    if (v.valA != null && v.valB != null)
                    {
                        if (!v.valA.Equals(v.valB))
                            variances.Add(v);
                    }
                }
            }
            return variances;
        }

        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }

        #endregion
    }
}
