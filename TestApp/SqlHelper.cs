
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infotronix.TestApp
{
    class SqlHelper
    {
        //public static string conStr = @"Data Source=51.255.68.69;Initial Catalog=admin_Infotronix;Uid=sa;Password=6Y7sO1YR;";
        public static string conStr = @"Data Source=149.56.23.109;Initial Catalog=admin_Infotronix;Uid=@web!!Inforotronix1;Password=!108!@@UserBhargav1;";

        protected static MEMBERS.SQLReturnValue ExecuteNoneQueryWithMessage(string ProceduerName, string[,] paramvalue, bool AddOutputparameter)
        {
            SqlConnection conn = new SqlConnection(conStr);
            MEMBERS.SQLReturnValue m = new MEMBERS.SQLReturnValue();
            try
            {
                SqlCommand cmd = new SqlCommand(ProceduerName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlParameter[] param = new SqlParameter[paramvalue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                    param[i] = new SqlParameter("@" + paramvalue[i, 0].ToString(), paramvalue[i, 1].ToString());

                cmd.Parameters.AddRange(param);
                if (AddOutputparameter)
                {
                    cmd.Parameters.Add("OUTVAL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("OUTMESSAGE", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                }
                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();
                m.OUTMESSAGE = cmd.Parameters["OUTMESSAGE"].Value.ToString();
                m.Outval = cmd.Parameters["OUTVAL"].Value.ToString();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (conn.State == ConnectionState.Open)conn.Close(); }
            return m;
        }


        public static MEMBERS.SQLReturnValue ExecuteNoneQueryWithPera(string QueryString, string[,] paramvalue)
        {
            SqlConnection conn = new SqlConnection(conStr);
            MEMBERS.SQLReturnValue m = new MEMBERS.SQLReturnValue();
            try
            {
                SqlCommand cmd = new SqlCommand(QueryString, conn);

                SqlParameter[] param = new SqlParameter[paramvalue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                    param[i] = new SqlParameter("@" + paramvalue[i, 0].ToString(), paramvalue[i, 1].ToString());

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(param);
                cmd.CommandTimeout = 0;
                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (conn.State == ConnectionState.Open)conn.Close(); }
            return m;
        }

        public static MEMBERS.SQLReturnValue ExecuteNoneQuery(string QueryString)
        {
            SqlConnection conn = new SqlConnection(conStr);
            MEMBERS.SQLReturnValue m = new MEMBERS.SQLReturnValue();
            try
            {
                SqlCommand cmd = new SqlCommand(QueryString, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (conn.State == ConnectionState.Open)conn.Close(); }
            return m;
        }

     
        protected static DataTable ExecuteProcedure(string ProcedureName, string[,] ParamValue)
        {
            SqlConnection MYCON = new SqlConnection(conStr);
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());

                SqlCommand cmd = new SqlCommand(ProcedureName, MYCON);
                cmd.Parameters.AddRange(param);
                cmd.Parameters.Add("@OUTVAL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.StoredProcedure;
                if (MYCON.State != ConnectionState.Open) MYCON.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
            return dt;
        }

        public static DataTable ExecuteProcedure(string Query)
        {
            SqlConnection MYCON = new SqlConnection(conStr);
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(Query, MYCON);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                if (MYCON.State != ConnectionState.Open) MYCON.Open();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
            return dt;
        }


        public static SqlDataReader ExecuteReader(string Query)
        {
            SqlConnection MYCON = new SqlConnection(conStr);
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(Query, MYCON);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                if (MYCON.State != ConnectionState.Open) MYCON.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader;
                }
            }
            catch (Exception ex)
            { throw ex; }
            //finally
            //{ if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
        }

        public static DataTable ExecuteReaderDT(string Query, DataTable dtNew)
        {
            using (SqlConnection MYCON = new SqlConnection(conStr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(Query, MYCON))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.Text;
                        if (MYCON.State != ConnectionState.Open) MYCON.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = dtNew.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                    row[i] = reader[i];
                                dtNew.Rows.Add(row);
                            }
                        }
                        return dtNew;
                    }
                }
                catch (Exception ex)
                { throw ex; }
                finally
                { if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
            }
        }

        public static DataSet ExecuteProcedureDS(string ProcedureName, string[,] ParamValue)
        {
            SqlConnection MYCON = new SqlConnection(conStr);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());

                SqlCommand cmd = new SqlCommand(ProcedureName, MYCON);
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddRange(param);
                cmd.Parameters.Add("@OUTVAL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@OUTMESSAGE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.StoredProcedure;
                if (MYCON.State != ConnectionState.Open) MYCON.Open();
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
            return ds;
        }

        public static void FillReport(DataSet ds, string TableName, string QueryString, object[,] ParameterValue)
        {
            using (SqlConnection MYCON = new SqlConnection(conStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(QueryString, MYCON);
                    SqlParameter[] param = new SqlParameter[ParameterValue.GetUpperBound(0) + 1];
                    for (int i = 0; i < param.Length; i++)
                        param[i] = new SqlParameter("@" + ParameterValue[i, 0].ToString(), ParameterValue[i, 1]);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(param);
                    SqlDataAdapter da = new SqlDataAdapter();
                    if (MYCON.State != ConnectionState.Open) MYCON.Open();
                    da.SelectCommand = cmd;
                    da.Fill(ds, TableName);
                }
                catch (Exception ex)
                { throw ex; }
                finally
                { if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
            }
        }
        //------------------------------------Parameter With Datatable----------------------------------------------
        public static MEMBERS.SQLReturnValue ExecuteProcedureWithDatatable(string ProcedureName, string[,] ParamValue, DataTable dtExamAnswer, string TableParamName)
        {
            SqlConnection MYCON = new SqlConnection(conStr);
            MEMBERS.SQLReturnValue m = new MEMBERS.SQLReturnValue();
            try
            {
                SqlCommand COMMAND = new SqlCommand(ProcedureName, MYCON);
                COMMAND.CommandTimeout = 0;
                COMMAND.CommandType = CommandType.StoredProcedure;
                SqlParameter[] param = new SqlParameter[ParamValue.GetUpperBound(0) + 1];
                for (int i = 0; i < param.Length; i++)
                    param[i] = new SqlParameter("@" + ParamValue[i, 0].ToString(), ParamValue[i, 1].ToString());

                COMMAND.Parameters.AddRange(param);
                if (dtExamAnswer != null)
                {
                    SqlParameter ParamTb = new SqlParameter("@" + TableParamName, dtExamAnswer);
                    ParamTb.SqlDbType = SqlDbType.Structured;
                    COMMAND.Parameters.Add(ParamTb);
                }
                COMMAND.Parameters.Add("OUTVAL", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                COMMAND.Parameters.Add("OUTMESSAGE", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;

                if (MYCON.State != ConnectionState.Open) MYCON.Open();
                COMMAND.ExecuteNonQuery();

                m.OUTMESSAGE = COMMAND.Parameters["OUTMESSAGE"].Value.ToString();
                m.Outval = COMMAND.Parameters["OUTVAL"].Value.ToString();
                //return m;
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { if (MYCON.State == ConnectionState.Open)MYCON.Close(); }
            return m;
        }
    }
}
