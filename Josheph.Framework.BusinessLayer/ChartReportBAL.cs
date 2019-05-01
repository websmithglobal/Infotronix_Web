using Josheph.Framework.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;
using BAL = Josheph.Framework.BusinessLayer;
using Josheph.Framework.Common.MySqlConnection;

namespace Josheph.Framework.BusinessLayer
{
    public class ChartReportBAL
    {
        SqlConnection sqlCon = new SqlConnection();

        public List<ENT.DashboardCards> GetDashboardCards(bool isRequiredToday, bool isRequiredLastOne)
        {
             string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d", sqlQuery;
            List<ENT.DashboardCards> m_return = new List<ENT.DashboardCards>();
            try
            {
                if (!GetConnection.isConnectionOpen)
                    GetConnection.OpenConnection(sqlCon);
                if (!isRequiredLastOne)
                {
                    sqlQuery = @"Select InverterSnNo, Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) AS EAC 
                            from DeviceFTPDetails WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + "')";
                    if (isRequiredToday)
                    {
                        sqlQuery += " AND DeviceFTPDetails.datatime >= '" + DateTime.Now.ToShortDateString() + " 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.ToShortDateString() + " 23:59:59'";
                        //sqlQuery += @" AND  DeviceFTPDetails.datatime >= '09/16/2017 00:00:00' AND DeviceFTPDetails.datatime <= '09/16/2017 23:59:59'";
                    }
                    sqlQuery += "group by InverterSnNo";
                }
                else
                {
                    sqlQuery = @"Select InverterSnNo, (Select top 1 Sum(CAST(REPLACE(A.EAC, CHAR(0), ' ') as numeric(18,2))) from DeviceFTPDetails A
                                WHERE A.InverterSnNo = DeviceFTPDetails.InverterSnNo
                                group by datatime
                                order by datatime desc) AS EAC  
                            from DeviceFTPDetails WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + "')";
                    if (isRequiredToday)
                    {
                        sqlQuery += " AND DeviceFTPDetails.datatime >= '" + DateTime.Now.ToShortDateString() + " 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.ToShortDateString() + " 23:59:59'";
                        //sqlQuery += @" AND  DeviceFTPDetails.datatime >= '09/16/2017 00:00:00' AND DeviceFTPDetails.datatime <= '09/16/2017 23:59:59'";
                    }
                    sqlQuery += "group by InverterSnNo";
                }
                SqlCommand sqlCMD = new SqlCommand();
                sqlCMD.Connection = GetConnection.GetDBConnection();
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.CommandText = sqlQuery;
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = sqlCMD.ExecuteReader())
                {
                    m_return = DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                }
                if (m_return == null)
                    throw new Exception();
                sqlCMD.Connection.Close();

            }
            catch (Exception ex)
            {
                m_return = new List<ENT.DashboardCards>();
            }
            return m_return;
        }

        public List<ENT.DashboardCards> GetChartData(DateTime fromdate, DateTime todate, bool isRequiredLastOne)
        {
             string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d", sqlQuery;
            List<ENT.DashboardCards> m_return = new List<ENT.DashboardCards>();
            try
            {
                if (!GetConnection.isConnectionOpen)
                    GetConnection.OpenConnection(sqlCon);
                if (!isRequiredLastOne)
                {
                    sqlQuery = @"select DeviceMaster.DeviceName AS InverterSnNo, Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) AS EAC 
                            FROM      DeviceFTPDetails LEFT OUTER JOIN 
                            DeviceMaster ON DeviceFTPDetails.InverterSnNo = DeviceMaster.DeviceSnnNo WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + "')";
                    sqlQuery += " AND DeviceFTPDetails.datatime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND DeviceFTPDetails.datatime <= '" + todate.ToShortDateString() + " 23:59:59'";
                    sqlQuery += " group by InverterSnNo , DeviceMaster.DeviceName";
                }
                else
                {
                    sqlQuery = @"select DeviceMaster.DeviceName AS InverterSnNo, (Select top 1 Sum(CAST(REPLACE(A.EAC, CHAR(0), ' ') as numeric(18,2))) from DeviceFTPDetails A
                                WHERE A.InverterSnNo = DeviceFTPDetails.InverterSnNo
                                group by datatime
                                order by datatime desc) AS EAC  
                            FROM      DeviceFTPDetails LEFT OUTER JOIN
                            DeviceMaster ON DeviceFTPDetails.InverterSnNo = DeviceMaster.DeviceSnnNo
                    WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + "')";
                    sqlQuery += " AND DeviceFTPDetails.datatime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND DeviceFTPDetails.datatime <= '" + todate.ToShortDateString() + " 23:59:59'";
                    sqlQuery += "group by InverterSnNo, DeviceMaster.DeviceName";
                }


                SqlCommand sqlCMD = new SqlCommand();
                sqlCMD.Connection = GetConnection.GetDBConnection();
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.CommandText = sqlQuery;
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = sqlCMD.ExecuteReader())
                {
                    m_return = DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                }
                if (m_return == null)
                    throw new Exception();
                sqlCMD.Connection.Close();
            }
            catch (Exception ex)
            {
                m_return = new List<ENT.DashboardCards>();
            }
            return m_return;
        }

        public List<ENT.DashboardCards> GetChartAreaData(DateTime fromdate, DateTime todate, bool isRequiredLastOne)
        {
             string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d", sqlQuery;
            List<ENT.DashboardCards> m_return = new List<ENT.DashboardCards>();
            try
            {
                if (!GetConnection.isConnectionOpen)
                    GetConnection.OpenConnection(sqlCon);
                if (!isRequiredLastOne)
                {
                    sqlQuery = @"select convert(varchar(10),(convert(date,datatime,103)),103) AS InverterSnNo, Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) AS EAC 
                            from DeviceFTPDetails WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + "')";
                    sqlQuery += " AND DeviceFTPDetails.datatime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND DeviceFTPDetails.datatime <= '" + todate.ToShortDateString() + " 23:59:59'";
                    sqlQuery += "group by convert(date,datatime,103) ORDER BY convert(date,datatime,103) Desc";
                }
                else
                {
                    sqlQuery = @";With temp as (
                                select  CAST(datatime AS DATE) as Date1,InverterSnNo,
                                (Select TOP 1 Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) 
                                from DeviceFTPDetails
                                WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + @"')
                                AND CAST(DeviceFTPDetails.datatime AS DATE) =CAST(invno.datatime AS DATE)
                                AND InverterSnNo = invno.InverterSnNo
                                group by InverterSnNo,datatime order by datatime desc) As Day1
                                FROM DeviceFTPDetails invno
                                WHERE invno.InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                                AND invno.datatime >= '" + fromdate.ToShortDateString() + @" 00:00:00' AND invno.datatime <= '" + todate.ToShortDateString() + @" 23:59:59'
                                    GROUP BY InverterSnNo, CAST(datatime AS DATE)
                                )select convert(varchar(10),Date1,103) AS InverterSnNo,sum(Day1) as EAC from temp
                                group by Date1 order by Date1 desc";
                }

                SqlCommand sqlCMD = new SqlCommand();
                sqlCMD.Connection = GetConnection.GetDBConnection();
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.CommandText = sqlQuery;
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = sqlCMD.ExecuteReader())
                {
                    m_return = DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                }
                if (m_return == null)
                    throw new Exception();
                sqlCMD.Connection.Close();
            }
            catch (Exception ex)
            {
                m_return = new List<ENT.DashboardCards>();
            }
            return m_return;
        }

        public List<ENT.InverterDateTable> Get7DaysTable()
        {
             string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d";
            List<ENT.InverterDateTable> m_return = new List<ENT.InverterDateTable>();
            try
            {

                if (!GetConnection.isConnectionOpen)
                    GetConnection.OpenConnection(sqlCon);
                string sqlQuery = @"select InverterSnNo,(Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day1,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.AddDays(-1).ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-1).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day2,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.AddDays(-2).ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-2).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day3,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.AddDays(-3).ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-3).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day4,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.AddDays(-4).ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-4).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day5,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.AddDays(-5).ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-5).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day6,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.AddDays(-6).ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-6).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day7,
                        (Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18, 2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.AddDays(-7).ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Total7Days
                        FROM DeviceFTPDetails invno
                        WHERE invno.InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        GROUP BY InverterSnNo";
                SqlCommand sqlCMD = new SqlCommand();
                sqlCMD.Connection = GetConnection.GetDBConnection();
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.CommandText = sqlQuery;
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = sqlCMD.ExecuteReader())
                {
                    m_return = DBHelper.CopyDataReaderToEntity<ENT.InverterDateTable>(dr);
                }
                if (m_return == null)
                    throw new Exception();
                sqlCMD.Connection.Close();
            }
            catch (Exception ex)
            {
                m_return = new List<ENT.InverterDateTable>();
            }
            return m_return;
        }

        public List<ENT.InverterDateTable> Get7DaysTable(bool isRequiredLastOne)
        {
             string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d";
            List<ENT.InverterDateTable> m_return = new List<ENT.InverterDateTable>();
            try
            {
                string strTemp;

                if (!isRequiredLastOne)
                {
                    strTemp = @"(Select Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) 
                        from DeviceFTPDetails
                        WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + @"')
                        AND DeviceFTPDetails.datatime >= '" + DateTime.Now.ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.ToShortDateString() + @" 23:59:59'
                        AND InverterSnNo = invno.InverterSnNo
                        group by InverterSnNo) As Day1 ";
                }
                else
                {
                    strTemp = @"(Select TOP 1 Sum(CAST(REPLACE(EAC, CHAR(0), ' ') as numeric(18,2))) 
                            from DeviceFTPDetails
                            WHERE InverterSnNo in (select devicesnnno from DeviceMaster where clientid='" + ClientID + @"')
                            AND DeviceFTPDetails.datatime >= '" + DateTime.Now.ToShortDateString() + @" 00:00:00' AND DeviceFTPDetails.datatime <= '" + DateTime.Now.ToShortDateString() + @" 23:59:59'
                            AND InverterSnNo = invno.InverterSnNo
                            group by InverterSnNo,datatime order by datatime desc) As Day1 ";
                }


                if (!GetConnection.isConnectionOpen)
                    GetConnection.OpenConnection(sqlCon);
                string sqlQuery = @"select InverterSnNo,DeviceMaster.DeviceName," + strTemp +
                            @"FROM      DeviceFTPDetails invno LEFT OUTER JOIN 
                            DeviceMaster ON invno.InverterSnNo = DeviceMaster.DeviceSnnNo 
                        WHERE invno.InverterSnNo in (select devicesnnno from DeviceMaster where clientid = '" + ClientID + @"')
                        GROUP BY InverterSnNo,DeviceMaster.DeviceName";
                SqlCommand sqlCMD = new SqlCommand();
                sqlCMD.Connection = GetConnection.GetDBConnection();
                sqlCMD.CommandType = System.Data.CommandType.Text;
                sqlCMD.CommandText = sqlQuery;
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = sqlCMD.ExecuteReader())
                {
                    m_return = DBHelper.CopyDataReaderToEntity<ENT.InverterDateTable>(dr);
                }
                if (m_return == null)
                    throw new Exception();
                sqlCMD.Connection.Close();
            }
            catch (Exception ex)
            {
                m_return = new List<ENT.InverterDateTable>();
            }
            return m_return;
        }
    }

    //public static class DBConnection
    //{
    //    public static Boolean isConnectionOpen { get; set; }

    //    public static SqlConnection GetDBConnection()
    //    {
    //        SqlConnection sqlcon = null;
    //        try
    //        {
    //            sqlcon = new SqlConnection(ReadConnectionString());
    //            sqlcon.Open();
    //            if (sqlcon.State == ConnectionState.Open)
    //            {
    //                isConnectionOpen = true;
    //            }
    //            else { isConnectionOpen = false; }
    //        }
    //        catch (Exception ex)
    //        {
    //            isConnectionOpen = false;
    //        }
    //        return sqlcon;
    //    }

    //    public static Boolean isServerAvailable()
    //    {
    //        Boolean blnResult = false;
    //        return blnResult;
    //    }

    //    public static Boolean OpenConnection(SqlConnection sqlCon)
    //    {
    //        try
    //        {
    //            if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
    //            if (sqlCon.State == ConnectionState.Open)
    //            {
    //                isConnectionOpen = true;
    //            }
    //            else
    //            {
    //                isConnectionOpen = false;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            isConnectionOpen = false;
    //        }
    //        return isConnectionOpen;
    //    }

    //    public static Boolean CloseConnection(SqlConnection sqlCon)
    //    {
    //        try
    //        {
    //            if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
    //            if (sqlCon.State == ConnectionState.Closed)
    //            {
    //                isConnectionOpen = false;
    //            }
    //            else { isConnectionOpen = true; }

    //        }
    //        catch (Exception ex)
    //        {
    //            isConnectionOpen = true;
    //        }
    //        return isConnectionOpen;
    //    }

    //    private static string ReadConnectionString()
    //    {
    //        return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    //    }
    //}
}
