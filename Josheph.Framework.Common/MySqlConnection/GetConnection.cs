using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Josheph.Framework.Common.MySqlConnection
{
    public static class GetConnection
    {
        [SettingsDescription("This properties will return connection is open of not.")]
        [DefaultSettingValue("false")]
        public static Boolean isConnectionOpen { get; set; }
        public static SqlConnection GetDBConnection()
        {
            SqlConnection sqlcon = null;
            try
            {
                sqlcon = new SqlConnection(ReadConnectionString());
                sqlcon.Open();
                if (sqlcon.State == ConnectionState.Open)
                {
                    isConnectionOpen = true;
                }
                else { isConnectionOpen = false; }
            }
            catch (Exception ex)
            {
                isConnectionOpen = false;
            }
            return sqlcon;
        }

        public static Boolean isServerAvailable()
        {
            Boolean blnResult = false;
            return blnResult;
        }

        public static Boolean OpenConnection(SqlConnection sqlCon)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                if (sqlCon.State == ConnectionState.Open)
                {
                    isConnectionOpen = true;
                }
                else
                {
                    isConnectionOpen = false;
                }
            }
            catch
            {
                isConnectionOpen = false;
            }
            return isConnectionOpen;
        }

        public static Boolean CloseConnection(SqlConnection sqlCon)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
                if (sqlCon.State == ConnectionState.Closed)
                {
                    isConnectionOpen = false;
                }
                else { isConnectionOpen = true; }

            }
            catch
            {
                isConnectionOpen = true;
            }
            return isConnectionOpen;
        }

        private static string ReadConnectionString()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }catch
            {
                return "Data Source=149.56.23.109;Initial Catalog=admin_Infotronix;Uid=@web!!Inforotronix1;Password=!108!@@UserBhargav1;";
            }
        }
    }
}
