using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotronix
{
    public class DBLogic
    {
        public static string ConnectionString = "Data Source=149.56.23.109;Initial Catalog=admin_Infotronix;Uid=@web!!Inforotronix1;Password=!108!@@UserBhargav1;Timeout=120";

        SqlConnection con = new SqlConnection(ConnectionString);

        public DataSet PlantMaster_GetForSendEmail(Guid PlantID)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "PlantMaster_GetForSendEmail",
                    Connection = con,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("PlantID", PlantID);

                if (con.State != ConnectionState.Open) { con.Open(); }

                SqlDataAdapter myDAP = new SqlDataAdapter(cmd);
                myDAP.Fill(ds);

                con.Close();
            }
            catch (Exception ee)
            {
                string Error = ee.Message;
            }
            finally
            {
                con.Close();
            }

            return ds;
        }

        public DataTable PlantMaster_GetAll()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "PlantMaster_GetAll",
                    Connection = con,
                    CommandType = CommandType.StoredProcedure
                };

                if (con.State != ConnectionState.Open) { con.Open(); }

                SqlDataAdapter myDAP = new SqlDataAdapter(cmd);
                myDAP.Fill(dt);

                con.Close();
            }
            catch (Exception ee)
            {
                string Error = ee.Message;
            }
            finally
            {
                con.Close();
            }

            return dt;
        }

        public void MessageSendLog_Insert(string Mobile, string Response, int IsSent)
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "MessageSendLog_Insert",
                    Connection = con,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("Mobile", Mobile);
                cmd.Parameters.AddWithValue("Response", Response);
                cmd.Parameters.AddWithValue("IsSent", IsSent);

                if (con.State != ConnectionState.Open) { con.Open(); }

                cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ee)
            {
                string Error = ee.Message;
            }
            finally
            {
                con.Close();
            }
        }
    }
}