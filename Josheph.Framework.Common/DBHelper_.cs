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
    public class DBHelper
    {
        private SqlConnection sqlCon = new SqlConnection();
        private SqlCommand sqlCMD = new SqlCommand();

        public DBHelper() { }

        private void GetConnection()
        {
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon = MySqlConnection.GetConnection.GetDBConnection();
                if (MySqlConnection.GetConnection.isConnectionOpen)
                {
                    sqlCMD.Connection = sqlCon;
                }
            }
        }

        ~DBHelper()
        {
            MySqlConnection.GetConnection.CloseConnection(sqlCon);
            sqlCMD.Dispose();
        }
        public void Disposed()
        {
            sqlCMD.Dispose();
            sqlCon.Close();
        }

        public object GetEnityByPrimaryKey(object objEntity)
        {
            object objResult = null;
            try
            {
                GetConnection();
                string strPrimaryKey = null;
                if (sqlCMD.Connection.State == ConnectionState.Open)
                {
                    if (sqlCMD.Connection.State == ConnectionState.Closed)
                        MySqlConnection.GetConnection.OpenConnection(sqlCon);
                    CommonMSSQL.ClearParameter(sqlCMD);

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

                    CommonMSSQL.AddParameter(sqlCMD, objEntity, strPrimaryKey);
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.CommandText = CommonMSSQL.PrepairSelectPrimaryKey(HelperMethod.GetTableName(objEntity).ToString(), objEntity);
                    using (SqlDataReader dr = sqlCMD.ExecuteReader())
                    {
                        objResult = FillEntityFromReader(dr, objEntity);
                        dr.Close();
                    }
                }
            }
            catch (Exception)
            {
                objResult = null;
                throw;
            }
            finally
            {
                if (sqlCon != null)
                {
                    if (sqlCon.State == ConnectionState.Open)
                    {
                        sqlCon.Close();
                        MySqlConnection.GetConnection.CloseConnection(sqlCon);
                    }
                }
            }
            return objResult;
        }
        public object GetEnityByPerameters(object objEntity, Dictionary<string, MyEnumration.Operation> parFields)
        {
            object objResult = null;
            try
            {
                GetConnection();
                if (sqlCMD.Connection.State == ConnectionState.Open)
                {
                    if (sqlCMD.Connection.State == ConnectionState.Closed)
                        MySqlConnection.GetConnection.OpenConnection(sqlCon);
                    CommonMSSQL.ClearParameter(sqlCMD);

                    for (int i = 0; i < parFields.Count; i++)
                    {
                        var el = parFields.ElementAt(i);
                        CommonMSSQL.AddParameter(sqlCMD, objEntity, el.Key);
                    }

                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.CommandText = CommonMSSQL.PrepairSelectPerameters(parFields, HelperMethod.GetTableName(objEntity), objEntity);
                    using (SqlDataReader dr = sqlCMD.ExecuteReader())
                    {
                        objResult = FillEntityFromReader(dr, objEntity);
                        dr.Close();
                    }

                }
            }
            catch (Exception)
            {
                objResult = null;
                throw;
            }
            finally
            {
                if (sqlCon != null)
                {
                    if (sqlCon.State == ConnectionState.Open)
                    {
                        sqlCon.Close();
                        MySqlConnection.GetConnection.CloseConnection(sqlCon);
                    }
                }
            }
            return objResult;
        }

        public SqlDataReader ExecuteReaderQuery(TTDictionaryQuery SQLDictionery, object objEntity)
        {
            SqlDataReader drResult = null;
            try
            {
                GetConnection();
                if (!MySqlConnection.GetConnection.isConnectionOpen)
                    sqlCon.Open();
                sqlCMD.Parameters.Clear();
                StringBuilder SQLQuery = new StringBuilder();

                SQLQuery.Append(SQLDictionery.SelectPart).AppendLine();
                SQLQuery.Append(SQLDictionery.TablePart).AppendLine();
                if (SQLDictionery.GroupPart.Trim() != "")
                    SQLQuery.Append(SQLDictionery.GroupPart).AppendLine();
                if (SQLDictionery.HavingPart.Trim() != "")
                    SQLQuery.Append(SQLDictionery.HavingPart).AppendLine();

                if (TTPagination.isPageing)
                {
                    SQLQuery.Append(string.Format("{0} {1}", SQLDictionery.OrderPart, HelperMethod.GetPageingString(SQLDictionery.OrderPart, objEntity))).AppendLine();
                }
                else
                {
                    SQLQuery.Append(SQLDictionery.OrderPart).AppendLine();
                }

                sqlCMD.CommandType = CommandType.Text;
                sqlCMD.CommandText = SQLQuery.ToString();
                SqlDataReader dr = sqlCMD.ExecuteReader();
                if (dr.HasRows)
                {
                    drResult = dr;
                    TTPagination.RecordCount = PagerRecordCount(SQLDictionery, null, objEntity);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (sqlCon != null)
                {
                    if (sqlCon.State == ConnectionState.Open)
                    {
                        sqlCon.Close();
                        MySqlConnection.GetConnection.CloseConnection(sqlCon);
                    }
                }
            }
            return drResult;
        }
        public SqlDataReader ExecuteReaderQuery(TTDictionaryQuery SQLDictionery, TTDictionary parDictionery, object objEntity)
        {
            SqlDataReader drResult = null;
            try
            {
                GetConnection();
                MySqlConnection.GetConnection.OpenConnection(sqlCon);
                sqlCMD.Parameters.Clear();
                CommonMSSQL.AddParameter(sqlCMD, parDictionery, objEntity);

                StringBuilder SQLQuery = new StringBuilder();

                SQLQuery.Append(SQLDictionery.SelectPart).AppendLine();
                SQLQuery.Append(SQLDictionery.TablePart).AppendLine();
                if (parDictionery.Count > 0)
                    SQLQuery.Append(HelperMethod.GetParameter(parDictionery, SQLQuery)).AppendLine();
                if (!string.IsNullOrEmpty(SQLDictionery.ParameterPart))
                {
                    if (SQLDictionery.ParameterPart.Trim() != "")
                    {
                        SQLQuery.Append(SQLDictionery.ParameterPart).AppendLine();
                    }
                }
                if (SQLDictionery.GroupPart != null)
                    SQLQuery.Append(SQLDictionery.GroupPart).AppendLine();
                if (SQLDictionery.HavingPart != null)
                    SQLQuery.Append(SQLDictionery.HavingPart).AppendLine();

                if (TTPagination.isPageing)
                {
                    if (TTPagination.PageSize != -1)
                    {
                        SQLQuery.Append(string.Format("{0} {1}", SQLDictionery.OrderPart, HelperMethod.GetPageingString(SQLDictionery.OrderPart, objEntity))).AppendLine();
                    }
                }
                else
                {
                    if (SQLDictionery.OrderPart != null)
                        SQLQuery.Append(SQLDictionery.OrderPart).AppendLine();
                }

                // sqlCMD.CommandTimeout = 0;
                sqlCMD.CommandTimeout = 380;
                sqlCMD.CommandType = CommandType.Text;
                sqlCMD.CommandText = SQLQuery.ToString();
                SqlDataReader dr = sqlCMD.ExecuteReader();
                TTPagination.RecordCount = PagerRecordCount(SQLDictionery, parDictionery, objEntity);
                drResult = dr;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                TTPagination.isPageing = false;
            }
            return drResult;
        }

        private long PagerRecordCount(TTDictionaryQuery SQLDictionery, TTDictionary parDictionery, object objEntity)
        {
            long lngResult = 0;
            StringBuilder sbCount = new StringBuilder();
            SqlCommand sqlCMDCount = new SqlCommand();
            sqlCMDCount.Connection = MySqlConnection.GetConnection.GetDBConnection();
            sbCount.Append("select total_count = COUNT(*) ").AppendLine();
            sbCount.Append(SQLDictionery.TablePart).AppendLine();
            if (parDictionery != null)
            {
                if (parDictionery.Count > 0)
                {
                    sqlCMDCount.Parameters.Clear();
                    CommonMSSQL.AddParameter(sqlCMDCount, parDictionery, objEntity);
                    sbCount.Append(HelperMethod.GetParameter(parDictionery, sbCount)).AppendLine();
                    if (SQLDictionery.ParameterPart != null)
                    {
                        sbCount.Append(SQLDictionery.ParameterPart.Replace("WHERE", "AND"));
                    }
                }
                else { sbCount.Append(SQLDictionery.ParameterPart); }
            }
            else { sbCount.Append(SQLDictionery.ParameterPart); }

            if (SQLDictionery.GroupPart != null)
                sbCount.Append(SQLDictionery.GroupPart).AppendLine();
            if (SQLDictionery.HavingPart != null)
                sbCount.Append(SQLDictionery.HavingPart).AppendLine();
            sqlCMDCount.CommandText = sbCount.ToString();
            object objCount = sqlCMDCount.ExecuteScalar();
            if (objCount != null)
            {
                lngResult = objCount == DBNull.Value ? 0 : Convert.ToInt64(objCount);
                TTPagination.PageCount = (long)Math.Ceiling((double)lngResult / TTPagination.PageSize);
            }
            sqlCMDCount.Connection.Close();
            sqlCMDCount.Dispose();
            return lngResult;
        }


        #region create list from datareader
        private object FillEntityFromReader(SqlDataReader dr, object objEntity)
        {
            object objResult = null;
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        foreach (var prop in objEntity.GetType().GetProperties())
                        {
                            TTAttributs attr = HelperMethod.GetTTAttributes(objEntity, prop.Name);
                            if (attr.isTableField)
                            {
                                int ordinal = dr.GetOrdinal(attr.FieldName);
                                object objValue = dr.GetValue(ordinal);
                                if (objValue != System.DBNull.Value)
                                {
                                    prop.SetValue(objEntity, objValue, null);
                                }
                            }
                        }
                    }
                    objResult = objEntity;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objResult;
        }

        //usage below method
        //List<ItemDetails> itemDetails = SQLHelper.CopyDataReaderToEntity<ItemDetails>(reader);
        public static TEntity CopyDataReaderToSingleEntity<TEntity>(IDataReader dataReader) where TEntity : class
        {
            TEntity entities = null;
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            while (dataReader.Read())
            {
                TEntity tempEntity = Activator.CreateInstance<TEntity>();
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        TTAttributs MyAttributs = HelperMethod.GetTTAttributes(tempEntity, property.Name);
                        if (MyAttributs.isSelectField)
                            SetValue<TEntity>(property, tempEntity, dataReader[property.Name]);
                    }
                    catch (Exception ex) { }
                }
                entities = tempEntity;
            }
            return entities;
        }

        public static List<TEntity> CopyDataReaderToEntity<TEntity>(IDataReader dataReader) where TEntity : class
        {
            List<TEntity> entities = new List<TEntity>();
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            while (dataReader.Read())
            {
                TEntity tempEntity = Activator.CreateInstance<TEntity>();
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        TTAttributs MyAttributs = HelperMethod.GetTTAttributes(tempEntity, property.Name);
                        if (MyAttributs.isSelectField)
                            SetValue<TEntity>(property, tempEntity, dataReader[property.Name]);
                    }
                    catch (Exception ex) { }
                }
                entities.Add(tempEntity);
            }
            return entities;
        }

        private static TEntity SetValue<TEntity>(PropertyInfo property, TEntity entity, object propertyValue) where TEntity : class
        {
            if (property.CanRead)
            {
                //if (property.PropertyType.Name != "String" &&
                //    property.PropertyType.Name != "Single" &&
                //    property.PropertyType.Name != "Int32" &&
                //    property.PropertyType.Name != "Int64" &&
                //    property.PropertyType.Name != "Guid" &&
                //    property.PropertyType.Name != "DateTime" &&
                //    property.PropertyType.Name != "Decimal" &&
                //    property.PropertyType.Name != "Boolean" &&
                //    property.PropertyType.Name != "LocationType" &&
                //    property.PropertyType.Name != "AlertCategory" &&
                //    property.PropertyType.Name != "AlertType" &&
                //    property.PropertyType.Name != "AppLogType" &&
                //    property.PropertyType.Name != "RegisterVia" &&
                //    property.PropertyType.Name != "CoinContentType" &&
                //    property.PropertyType.Name != "CoinPatternType" &&
                //    property.PropertyType.Name != "MasterStatus" &&
                //    property.PropertyType.Name != "MockQuestionType")
                //    return entity;
                if (propertyValue == null)
                {
                    if (property.PropertyType.Name == "String")
                        propertyValue = "";
                    else
                        propertyValue = 0;
                }
                if (property.CanWrite)
                {
                    if (propertyValue != DBNull.Value)
                    {
                        if (property.PropertyType.Name == "Single")
                            property.SetValue(entity, Convert.ToSingle(propertyValue), null);
                        else if (property.PropertyType.Name == "Int32")
                            property.SetValue(entity, Convert.ToInt32(propertyValue), null);
                        else if (property.PropertyType.Name == "Int64")
                            property.SetValue(entity, Convert.ToInt64(propertyValue), null);
                        else { property.SetValue(entity, propertyValue, null); }
                        //else if (property.PropertyType.Name == "String")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "Boolean")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "DateTime")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "Guid")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "Decimal")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "LocationType")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "AlertCategory")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "AlertType")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "AppLogType")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "RegisterVia")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "CoinContentType")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "CoinPatternType")
                        //    property.SetValue(entity, propertyValue, null);
                        //else if (property.PropertyType.Name == "MockQuestionType")
                        //    property.SetValue(entity, propertyValue, null);
                    }
                }
            }
            return entity;
        }

        #endregion

        public static List<T> Query<T>(string query) where T : new()
        {
            List<T> res = new List<T>();
            SqlCommand q = new SqlCommand();
            SqlDataReader r = q.ExecuteReader();
            while (r.Read())
            {
                T t = new T();

                for (int inc = 0; inc < r.FieldCount; inc++)
                {
                    Type type = t.GetType();
                    PropertyInfo prop = type.GetProperty(r.GetName(inc));
                    prop.SetValue(t, r.GetValue(inc), null);
                }

                res.Add(t);
            }
            r.Close();

            return res;

        }
    }
}
