using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM = Josheph.Framework.Common;
using ENT = Josheph.Framework.Entity;


namespace Josheph.Framework.DataLayer
{
    public class DeviceDataDAL
    {
        #region Declare Common Object
        List<ENT.DeviceDataSUB> lstEntity = new List<ENT.DeviceDataSUB>();
        ENT.DeviceDataSUB objEntity = new ENT.DeviceDataSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion
        public DeviceDataDAL()
        { parFields.Clear(); }

        public List<ENT.LastActivityMinutes> GetPlantActiveMinutes(Guid UserID)
        {
            List<ENT.LastActivityMinutes> lstResult = new List<Framework.Entity.LastActivityMinutes>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                //QueryDisctionery.SelectPart = "SELECT TOP 1 DATEDIFF(MINUTE,CONVERT(DATETIME, CONVERT(CHAR(8), DeviceData.DeviceDate, 112) + ' ' + CONVERT(CHAR(8), DeviceData.DeviceTime, 108)),getdate()) AS LastActMinutes ";
                QueryDisctionery.SelectPart = "SELECT TOP 1 DATEDIFF(MINUTE,DeviceData.DeviceDateTime,getdate()) AS LastActMinutes, DeviceData.DeviceDateTime AS LastDateTime ";
                QueryDisctionery.TablePart = @"FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.ParameterPart = @"WHERE   (PlantMaster.AspNetUserID ='" + UserID.ToString() + "') and DeviceData.devicedate='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                QueryDisctionery.OrderPart = @"ORDER BY DeviceData.DeviceDateTime Desc ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.LastActivityMinutes>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }
        
        public List<ENT.DeviceDataSUB> GetList(string SubDeviceId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                QueryDisctionery.SelectPart = @"SELECT        DeviceData.DeviceDataID, DeviceData.SubDeviceId, DeviceData.UPV1, DeviceData.UPV2, DeviceData.UPV3, DeviceData.UPV4, DeviceData.UPV5, DeviceData.UPV6, DeviceData.UPV7, DeviceData.UPV8, DeviceData.UPV9, 
                         DeviceData.UPV10, DeviceData.UPV11, DeviceData.UPV12, DeviceData.IPV1, DeviceData.IPV2, DeviceData.IPV3, DeviceData.IPV4, DeviceData.IPV5, DeviceData.IPV6, DeviceData.IPV7, DeviceData.IPV8, DeviceData.IPV9, 
                         DeviceData.IPV10, DeviceData.IPV11, DeviceData.IPV12, DeviceData.UAC1, DeviceData.UAC2, DeviceData.UAC3, DeviceData.IAC1, DeviceData.IAC2, DeviceData.IAC3, DeviceData.Status, DeviceData.Error, DeviceData.Temp, 
                         DeviceData.Cos, DeviceData.Fac, DeviceData.Pac, DeviceData.Eac, DeviceData.Qac, DeviceData.DeviceDate, CONVERT(varchar, DeviceData.DeviceDateTime, 108) AS DeviceTime, DeviceData.TotalEnergy, 
                         DeviceData.TotalRealPower, DeviceData.DeviceDateTime, DeviceData.CreatedDate, SubDeviceMaster.Make ";
                QueryDisctionery.TablePart = @"FROM            DeviceData LEFT OUTER JOIN
                         SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId ";
                QueryDisctionery.ParameterPart = "where DeviceData.SubDeviceId = '" + SubDeviceId + "' and DeviceDateTime >= '" + FromDate.ToString("MM/dd/yyyy HH:mm:ss") + "' AND DeviceDateTime <='" + ToDate.ToString("MM/dd/yyyy HH:mm:ss") + "'";
                QueryDisctionery.OrderPart = " Order By DeviceDateTime Desc";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {

                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.DeviceDataSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.DashboardCards> GetDashboardCards(bool isRequiredToday, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SerialNo,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "') AS EAC  ";
                }
                else { QueryDisctionery.SelectPart = "SELECT SubDeviceMaster.SerialNo , SUM(DeviceData.Eac) AS EAC "; }

                if (isRequiredToday)
                {
                    QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }
                else { QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "'"; }

                QueryDisctionery.TablePart = @"FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.GroupPart = @"GROUP By SubDeviceMaster.SerialNo ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsExcluded(bool isRequiredToday, bool isRequiredLastOne, Guid UserID, String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SerialNo,
                            (Select TOP 1 DD.Eac
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ORDER By DD.DeviceDateTime Desc) AS EAC  ";
                }
                else { QueryDisctionery.SelectPart = "SELECT SubDeviceMaster.SerialNo , SUM(DeviceData.Eac) AS EAC "; }

                if (isRequiredToday)
                {
                    QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo NOT IN "+Exclude;
                }
                else { QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND SubDeviceMaster.SerialNo NOT IN " + Exclude; }

                QueryDisctionery.TablePart = @"FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.GroupPart = @"GROUP By SubDeviceMaster.SerialNo ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsTop(bool isRequiredToday, bool isRequiredLastOne, Guid UserID, String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SerialNo,
                            (Select TOP 1 DD.Eac
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ORDER By DD.Eac Desc) AS EAC  ";
                }
                else { QueryDisctionery.SelectPart = "SELECT SubDeviceMaster.SerialNo , SUM(DeviceData.Eac) AS EAC "; }

                if (isRequiredToday)
                {
                    QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo IN " + Exclude;
                }
                else { QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND SubDeviceMaster.SerialNo IN " + Exclude; }

                QueryDisctionery.TablePart = @"FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.GroupPart = @"GROUP By SubDeviceMaster.SerialNo ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }
        
        public List<ENT.DashboardCards> GetDashboardCardsInverterTable(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    //(Select TOP 1 DD.Eac
                    //(Select MAX(DD.Eac)
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SerialNo,
                        (Select MAX(DD.Eac)      
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "') AS EAC  ";
                }
                else { QueryDisctionery.SelectPart = "SELECT SubDeviceMaster.SerialNo , SUM(DeviceData.Eac) AS EAC "; }
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "'";



                QueryDisctionery.TablePart = @" FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.GroupPart = @"GROUP By SubDeviceMaster.SerialNo,SubDeviceMaster.SubDeviceId,MainDeviceMaster.Make ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsInverterTableExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SerialNo,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "') AS EAC  ";
                }
                else { QueryDisctionery.SelectPart = "SELECT SubDeviceMaster.SerialNo , SUM(DeviceData.Eac) AS EAC "; }
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo NOT IN "+Excluded;
                
                QueryDisctionery.TablePart = @" FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.GroupPart = @"GROUP By SubDeviceMaster.SerialNo,SubDeviceMaster.SubDeviceId,MainDeviceMaster.Make ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsInverterTableTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    //QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName,
                    //        (Select TOP 1 DD.Eac
                    //        FROM      DeviceData DD RIGHT OUTER JOIN
                    //                SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                    //                MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                    //                PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                    //        WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' ORDER By DD.Eac Desc) AS EAC  ";
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "') AS EAC";
                }
                else { QueryDisctionery.SelectPart = "SELECT SubDeviceMaster.SerialNo , SUM(DeviceData.Eac) AS EAC "; }
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo IN " + Excluded;

                QueryDisctionery.TablePart = @" FROM      DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId";
                QueryDisctionery.GroupPart = @"GROUP By SubDeviceMaster.SubDeviceName,SubDeviceMaster.SerialNo,SubDeviceMaster.SubDeviceId,MainDeviceMaster.Make ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartData(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    //QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                    //            (Select TOP 1 DD.Eac
                    //            FROM      DeviceData DD RIGHT OUTER JOIN
                    //                SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                    //                MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                    //                PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                    //            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' ORDER By DD.DeviceDateTime Desc) AS EAC ";

                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                            (Select TOP 1 DD.Eac
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' ORDER By DD.DeviceDateTime Desc) AS EAC";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS EAC ";
                }

                QueryDisctionery.TablePart = @"FROM    DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "'";
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SerialNo,SubDeviceMaster.SubDeviceName,SubDeviceMaster.PerformsOfPlantUniteVolume ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartDataExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                                (Select MAX(DD.Eac)
                                FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                                WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "') AS EAC ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS EAC ";
                }

                QueryDisctionery.TablePart = @"FROM    DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo NOT IN "+Exclude;
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SerialNo,SubDeviceMaster.SubDeviceName,SubDeviceMaster.PerformsOfPlantUniteVolume ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartDataTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    //QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                    //            (Select TOP 1 DD.Eac
                    //            FROM      DeviceData DD RIGHT OUTER JOIN
                    //                SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                    //                MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                    //                PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                    //            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' ORDER By DD.Eac Desc) AS EAC ";
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DD.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "') AS EAC  ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS EAC ";
                }

                QueryDisctionery.TablePart = @"FROM    DeviceData RIGHT OUTER JOIN
                            SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                            MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo IN " + Exclude;
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SerialNo,SubDeviceMaster.SubDeviceName,SubDeviceMaster.PerformsOfPlantUniteVolume ";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartAreaData(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @";With temp as (
                            select  CAST(invno.DeviceDateTime AS DATE) as Date1,invno.SubDeviceId ,
                                    (Select top 1 Sum(EAC)  from DeviceData as SB WHERE SB.SubDeviceId in (select SubDeviceMaster.SubDeviceId from 
								                            SubDeviceMaster RIGHT OUTER JOIN  MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
								                            where PlantMaster.AspNetUserID ='" + UserID.ToString() + @"') AND CAST(SB.DeviceDateTime AS DATE) =CAST(invno.DeviceDateTime AS DATE) AND EAC <> 0

                                    AND SubDeviceId  = invno.SubDeviceId 
                                    group by SB.SubDeviceId,SB.DeviceDateTime order by SB.DeviceDateTime desc) As Day1

                                    FROM DeviceData invno
                                    WHERE invno.SubDeviceId  in (select SubDeviceMaster.SubDeviceId from 
								                            SubDeviceMaster RIGHT OUTER JOIN  MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
								                            where PlantMaster.AspNetUserID ='" + UserID.ToString() + @"')
                                    AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59'
                                        GROUP BY SubDeviceId, CAST(invno.DeviceDateTime AS DATE)
	                            )
                            select convert(varchar(10), Date1, 103) AS SerialNo, sum(Day1) as EAC from temp group by Date1 order by Date1 ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT convert(varchar(10),DeviceData.DeviceDate,103) AS SerialNo, Sum(DeviceData.Eac) AS EAC ";
                    QueryDisctionery.TablePart = @"FROM    DeviceData RIGHT OUTER JOIN
                                SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                                MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                    QueryDisctionery.ParameterPart = @"WHERE DeviceData.DeviceDate is not null AND PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "'";
                    QueryDisctionery.GroupPart = @"GROUP BY DeviceData.DeviceDate ";
                    QueryDisctionery.OrderPart = @"ORDER BY DeviceData.DeviceDate ";

                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartAreaDataExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @";With temp as (
                            select  CAST(invno.DeviceDateTime AS DATE) as Date1,invno.SubDeviceId ,
                                    (Select top 1 Sum(EAC)  from DeviceData as SB WHERE SB.SubDeviceId in (select SubDeviceMaster.SubDeviceId from 
								                            SubDeviceMaster RIGHT OUTER JOIN  MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
								                            where PlantMaster.AspNetUserID ='" + UserID.ToString() + @"') AND CAST(SB.DeviceDateTime AS DATE) =CAST(invno.DeviceDateTime AS DATE)

                                    AND SubDeviceId  = invno.SubDeviceId 
                                    group by SB.SubDeviceId,SB.DeviceDateTime order by SB.DeviceDateTime desc) As Day1

                                    FROM DeviceData invno
                                    WHERE invno.SubDeviceId  in (select SubDeviceMaster.SubDeviceId from 
								                            SubDeviceMaster RIGHT OUTER JOIN  MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
								                            where PlantMaster.AspNetUserID ='" + UserID.ToString() + @"')
                                    AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59'
                                        GROUP BY SubDeviceId, CAST(invno.DeviceDateTime AS DATE)
	                            )
                            select convert(varchar(10), Date1, 103) AS SerialNo, sum(Day1) as EAC from temp group by Date1 order by Date1 ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT convert(varchar(10),DeviceData.DeviceDate,103) AS SerialNo, Sum(DeviceData.Eac) AS EAC ";
                    QueryDisctionery.TablePart = @"FROM    DeviceData RIGHT OUTER JOIN
                                SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                                MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                    QueryDisctionery.ParameterPart = @"WHERE DeviceData.DeviceDate is not null AND PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo NOT IN "+Excluded;
                    QueryDisctionery.GroupPart = @"GROUP BY DeviceData.DeviceDate ";
                    QueryDisctionery.OrderPart = @"ORDER BY DeviceData.DeviceDate ";

                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartAreaDataTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @";With temp as (
                            select  CAST(invno.DeviceDateTime AS DATE) as Date1,invno.SubDeviceId ,
                                    (Select top 1 Sum(EAC)  from DeviceData as SB WHERE SB.SubDeviceId in (select SubDeviceMaster.SubDeviceId from 
								                            SubDeviceMaster RIGHT OUTER JOIN  MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
								                            where PlantMaster.AspNetUserID ='" + UserID.ToString() + @"') AND CAST(SB.DeviceDateTime AS DATE) =CAST(invno.DeviceDateTime AS DATE)

                                    AND SubDeviceId  = invno.SubDeviceId 
                                    group by SB.SubDeviceId,SB.DeviceDateTime order by SB.DeviceDateTime desc) As Day1

                                    FROM DeviceData invno
                                    WHERE invno.SubDeviceId  in (select SubDeviceMaster.SubDeviceId from 
								                            SubDeviceMaster RIGHT OUTER JOIN  MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                                            PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
								                            where PlantMaster.AspNetUserID ='" + UserID.ToString() + @"')
                                    AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59'
                                        GROUP BY SubDeviceId, CAST(invno.DeviceDateTime AS DATE)
	                            )
                            select convert(varchar(10), Date1, 103) AS SerialNo, sum(Day1) as EAC from temp group by Date1 order by Date1 ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT convert(varchar(10),DeviceData.DeviceDate,103) AS SerialNo, Sum(DeviceData.Eac) AS EAC ";
                    QueryDisctionery.TablePart = @"FROM    DeviceData RIGHT OUTER JOIN
                                SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN
                                MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                    QueryDisctionery.ParameterPart = @"WHERE DeviceData.DeviceDate is not null AND PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("yyyy-MM-dd") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("yyyy-MM-dd") + "' AND SubDeviceMaster.SerialNo IN " + Excluded;
                    QueryDisctionery.GroupPart = @"GROUP BY DeviceData.DeviceDate ";
                    QueryDisctionery.OrderPart = @"ORDER BY DeviceData.DeviceDate ";

                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCards>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergy(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            try
            {
                string Make = "";
                QueryDisctionery.SelectPart = @"select top 1 Make from MainDeviceMaster as A left join PlantMaster as B ON A.PlantId = B.PlantId  where  B.AspNetUserID = '" + hdLoginID + "'";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    while (dr.Read())
                    {
                        Make = dr["Make"].ToString();
                    }
                }

                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();

                string EacVal = "";
                if (Make == "Schneider" || Make == "CSK Infotronix")
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID =  '" + hdLoginID + "' and ReportDate = '" + todate.ToShortDateString() + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as Eac1 " : " Eac  as Eac1";

                        QueryDisctionery.SelectPart = @" ;with temp as(select rank() OVER (ORDER BY invno.DeviceDateTime) as 'DataRank', CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,"
                                            + EacVal + ",PerFormsOfPlantUniteVolume ,DeviceDateTime from DeviceData as invno left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId "
                                            + "where SerialNo = '" + SubDeviceID + "' AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59') "
                                            + "select a.SerialNo,isnull(convert(varchar(10),(A.Eac1 - B.Eac1)),'0.0000') as EAC,A.PerFormsOfPlantUniteVolume from temp as A left join temp as B on A.DataRank - 1 = B.DataRank order by A.DeviceDateTime";
                    }
                }
                else
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID = '" + hdLoginID + "' and ReportDate = '" + todate.ToShortDateString() + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                        QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                            + "AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    }
                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCardsNew>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyAPI(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            try
            {
                string Make = "";
                QueryDisctionery.SelectPart = @"select top 1 Make from MainDeviceMaster as A left join PlantMaster as B ON A.PlantId = B.PlantId  where  B.AspNetUserID = '" + hdLoginID + "'";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    while (dr.Read())
                    {
                        Make = dr["Make"].ToString();
                    }
                }

                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();

                string EacVal = "";
                if (Make == "Schneider" || Make == "CSK Infotronix")
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        // QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID =  '" + hdLoginID + "' and ReportDate = '" + todate.ToString("dd/MMM/yyyy") + "' and  IsConverted = " + checkbox6a + "";
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID =  '" + hdLoginID + "' and ReportDate = '" + todate.ToShortDateString() + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as Eac1 " : " Eac  as Eac1";

                        //QueryDisctionery.SelectPart = @" ;with temp as(select rank() OVER (ORDER BY invno.DeviceDateTime) as 'DataRank', CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,"
                        //                    + EacVal + ",PerFormsOfPlantUniteVolume ,DeviceDateTime from DeviceData as invno left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId "
                        //                    + "where SerialNo = '" + SubDeviceID + "' AND invno.DeviceDateTime >= '" + fromdate.ToString("dd/MMM/yyyy") + " 00:00:00' AND invno.DeviceDateTime <= '" + todate.ToString("dd/MMM/yyyy") + @" 23:59:59') "
                        //                    + "select a.SerialNo,isnull(convert(varchar(10),(A.Eac1 - B.Eac1)),'0.0000') as EAC,A.PerFormsOfPlantUniteVolume from temp as A left join temp as B on A.DataRank - 1 = B.DataRank order by A.DeviceDateTime";
                        
                        QueryDisctionery.SelectPart = @" ;with temp as(select rank() OVER (ORDER BY invno.DeviceDateTime) as 'DataRank', CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,"
                                          + EacVal + ",PerFormsOfPlantUniteVolume ,DeviceDateTime from DeviceData as invno left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId "
                                          + "where SerialNo = '" + SubDeviceID + "' AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59') "
                                          + "select a.SerialNo,isnull(convert(varchar(10),(A.Eac1 - B.Eac1)),'0.0000') as EAC,A.PerFormsOfPlantUniteVolume from temp as A left join temp as B on A.DataRank - 1 = B.DataRank order by A.DeviceDateTime";
                    }
                }
                else
                {
                    //if (SubDeviceID == "DIV000000")
                    //{
                    //    QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID = '" + hdLoginID + "' and ReportDate = '" + todate.ToString("dd/MMM/yyyy") + "' and  IsConverted = " + checkbox6a + "";
                    //}
                    //else
                    //{
                    //    EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                    //    QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                    //        + "AND invno.DeviceDateTime >= '" + fromdate.ToString("dd/MMM/yyyy") + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToString("dd/MMM/yyyy") + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    //}
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID = '" + hdLoginID + "' and ReportDate = '" + todate.ToShortDateString() + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                        QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                            + "AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    }
                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCardsNew>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyExcludedAPI(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a, String Excluded)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            try
            {
                string Make = "";
                QueryDisctionery.SelectPart = @"select top 1 Make from MainDeviceMaster as A left join PlantMaster as B ON A.PlantId = B.PlantId  where  B.AspNetUserID = '" + hdLoginID + "'";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    while (dr.Read())
                    {
                        Make = dr["Make"].ToString();
                    }
                }

                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();

                string EacVal = "";
                if (Make == "Schneider")
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID =  '" + hdLoginID + "' and ReportDate = '" + todate.ToString("dd/MMM/yyyy") + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as Eac1 " : " Eac  as Eac1";

                        QueryDisctionery.SelectPart = @" ;with temp as(select rank() OVER (ORDER BY invno.DeviceDateTime) as 'DataRank', CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,"
                                            + EacVal + ",PerFormsOfPlantUniteVolume ,DeviceDateTime from DeviceData as invno left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId "
                                            + "where SerialNo = '" + SubDeviceID + "' AND invno.DeviceDateTime >= '" + fromdate.ToString("dd/MMM/yyyy") + " 00:00:00' AND invno.DeviceDateTime <= '" + todate.ToString("dd/MMM/yyyy") + @" 23:59:59') "
                                            + "select a.SerialNo,isnull(convert(varchar(10),(A.Eac1 - B.Eac1)),'0.0000') as EAC,A.PerFormsOfPlantUniteVolume from temp as A left join temp as B on A.DataRank - 1 = B.DataRank order by A.DeviceDateTime";
                    }
                }
                else
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID = '" + hdLoginID + "' and ReportDate = '" + todate.ToString("dd/MMM/yyyy") + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                        QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                            + "AND invno.DeviceDateTime >= '" + fromdate.ToString("dd/MMM/yyyy") + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToString("dd/MMM/yyyy") + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    }
                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCardsNew>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyExcluded(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a,String Excluded)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            try
            {
                string Make = "";
                QueryDisctionery.SelectPart = @"select top 1 Make from MainDeviceMaster as A left join PlantMaster as B ON A.PlantId = B.PlantId  where  B.AspNetUserID = '" + hdLoginID + "'";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    while (dr.Read())
                    {
                        Make = dr["Make"].ToString();
                    }
                }

                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();

                string EacVal = "";
                if (Make == "Schneider" || Make == "CSK Infotronix")
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID =  '" + hdLoginID + "' and ReportDate = '" + todate.ToShortDateString() + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as Eac1 " : " Eac  as Eac1";

                        QueryDisctionery.SelectPart = @" ;with temp as(select rank() OVER (ORDER BY invno.DeviceDateTime) as 'DataRank', CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,"
                                            + EacVal + ",PerFormsOfPlantUniteVolume ,DeviceDateTime from DeviceData as invno left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId "
                                            + "where SerialNo = '" + SubDeviceID + "' AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59') "
                                            + "select a.SerialNo,isnull(convert(varchar(10),(A.Eac1 - B.Eac1)),'0.0000') as EAC,A.PerFormsOfPlantUniteVolume from temp as A left join temp as B on A.DataRank - 1 = B.DataRank order by A.DeviceDateTime";
                    }
                }
                else
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT SubDeviceId,SubDeviceName as SerialNo,DispalyData AS Eac FROM DailyBallAllChart where AspNetUserID = '" + hdLoginID + "' and ReportDate = '" + todate.ToShortDateString() + "' and  IsConverted = " + checkbox6a + "";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                        QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                            + "AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    }
                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.DashboardCardsNew>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> InsertDailyAllDataReport(DateTime fromdate, DateTime todate)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            PlantMasterDAL clsDAL = new PlantMasterDAL();
            List<ENT.PlantMasterSUB> lstEntity = new List<ENT.PlantMasterSUB>();

            //GetAllDailyEnergyService(Guid.Parse("DBF3D275-0110-4D03-A519-7A777D18020E"), fromdate, todate, false, "DIV000000", "0");
            //GetAllDailyEnergyService(Guid.Parse("DBF3D275-0110-4D03-A519-7A777D18020E"), fromdate, todate, false, "DIV000000", "1");

            lstEntity = clsDAL.GetList(string.Empty, 1, 0);
            for (int i = 0; i < lstEntity.Count; i++)
            {
                GetAllDailyEnergyService(lstEntity[i].AspNetUserID, fromdate, todate, false, "DIV000000", "0");
                GetAllDailyEnergyService(lstEntity[i].AspNetUserID, fromdate, todate, false, "DIV000000", "1");
            }
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetAllDailyEnergyService(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            try
            {
                string Make = "";
                QueryDisctionery.SelectPart = @"select top 1 Make from MainDeviceMaster as A left join PlantMaster as B ON A.PlantId = B.PlantId  where  B.AspNetUserID = '" + hdLoginID + "'";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                { while (dr.Read()) { Make = dr["Make"].ToString(); } }

                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();

                string EacVal = "";
                
                if (Make == "Schneider")
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = "SELECT top 20 A1.SubDeviceId,A1.SubDeviceName as SerialNo,ISNULL(STUFF((SELECT ', ' + AllTime + '|' + Eac  "
                             + " from fnGetDeviceDateTimeWise_Schneider(A1.SerialNo, '" + fromdate.ToShortDateString() + " 00:00:00', '" + todate.ToShortDateString() + @" 23:59:59'," + checkbox6a + ")"
                            + " FOR XML PATH('')), 1, 1, ''), '0') AS Eac  From SubDeviceMaster as A1   join MainDeviceMaster as B1 on A1.DeviceID = B1.DeviceID"
                            + " join PlantMaster as C1 ON B1.PlantId = C1.PlantId    where C1.AspNetUserID = '" + hdLoginID + "'";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as Eac1 " : " Eac  as Eac1";

                        QueryDisctionery.SelectPart = @" ;with temp as(select rank() OVER (ORDER BY invno.DeviceDateTime) as 'DataRank', CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,"
                                            + EacVal + ",PerFormsOfPlantUniteVolume ,DeviceDateTime from DeviceData as invno left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId "
                                            + "where SerialNo = '" + SubDeviceID + "' AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00' AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59') "
                                            + "select a.SerialNo,isnull(convert(varchar(10),(A.Eac1 - B.Eac1)),'0.0000') as EAC,A.PerFormsOfPlantUniteVolume from temp as A left join temp as B on A.DataRank - 1 = B.DataRank order by A.DeviceDateTime";
                    }
                }
                else if(Make == "CSK Infotronix")
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = @"SELECT A1.SubDeviceId,A1.SubDeviceName as SerialNo,"
                          + " ISNULL(STUFF((SELECT ', ' + AllTime + '|' + Eac"
                          + " from fnGetDeviceDateTimeWise_Csk(A1.SerialNo,'" + todate.ToShortDateString() + @" 00:00:00','" + todate.ToShortDateString() + @" 23:59:59'," + checkbox6a + ")"
                          + " FOR XML PATH('')), 1, 1, ''), '0') AS Eac  From SubDeviceMaster as A1   join MainDeviceMaster as B1 on A1.DeviceID = B1.DeviceID "
                          + " join PlantMaster as C1 ON B1.PlantId = C1.PlantId    where C1.AspNetUserID = '" + hdLoginID + "'";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                        QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                            + "AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    }
                }
                else
                {
                    if (SubDeviceID == "DIV000000")
                    {
                        QueryDisctionery.SelectPart = @"SELECT A1.SubDeviceId,A1.SubDeviceName as SerialNo,"
                          + " ISNULL(STUFF((SELECT ', ' + AllTime + '|' + Eac"
                          + " from fnGetDeviceDateTimeWise(A1.SerialNo,'" + todate.ToShortDateString() + @" 00:00:00','" + todate.ToShortDateString() + @" 23:59:59'," + checkbox6a + ")"
                          + " FOR XML PATH('')), 1, 1, ''), '0') AS Eac  From SubDeviceMaster as A1   join MainDeviceMaster as B1 on A1.DeviceID = B1.DeviceID "
                          + " join PlantMaster as C1 ON B1.PlantId = C1.PlantId    where C1.AspNetUserID = '" + hdLoginID + "'";
                    }
                    else
                    {
                        EacVal = checkbox6a == "1" ? "  (CASE WHEN ISNULL(PerFormsOfPlantUniteVolume,0) = 0 then Eac ELSE (Eac /  ISNULL(PerFormsOfPlantUniteVolume,0)) END) as EAC " : " EAC ";
                        QueryDisctionery.SelectPart = @" select CONVERT(varchar(10),DeviceDateTime,108) AS SerialNo,CONVERT(varchar(10), " + EacVal + ") as EAC from DeviceData as invno  left join SubDeviceMaster as B on invno.SubDeviceId = B.SubDeviceId where SerialNo = '" + SubDeviceID + "'"
                            + "AND invno.DeviceDateTime >= '" + fromdate.ToShortDateString() + " 00:00:00'  AND invno.DeviceDateTime <= '" + todate.ToShortDateString() + @" 23:59:59' order by  invno.DeviceDateTime  asc";
                    }
                }

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    while (dr.Read())
                    {
                        ENT.DailyBallChartEntry modal = new ENT.DailyBallChartEntry();
                        modal.DailyBallAllChartID = Guid.NewGuid();
                        modal.AspNetUserID = hdLoginID;
                        modal.ReportDate = fromdate;
                        modal.DispalyData = dr["Eac"].ToString();
                        modal.SubDeviceID = Guid.Parse(dr["SubDeviceId"].ToString());
                        modal.SubDeviceName = dr["SerialNo"].ToString();
                        modal.IsConverted = checkbox6a == "1" ? true : false;
                        CRUDOperation objDAL = new CRUDOperation();
                        //Dictionary<string, bool> dctFields = new Dictionary<string, bool>();
                        //dctFields.Add("SubDeviceId", true);
                        //objDAL.DeleteByParameter(dctFields, modal);
                        objDAL.Insert(modal);
                    }
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.InverterDateTable> Get7DaysTable(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    //(Select MAX(DD.Eac)
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName,SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate + "' AND DD.DeviceDate <= '" + todate + "') AS Day1  ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName, SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS Day1 ";
                }

                QueryDisctionery.TablePart = @" ,MainDeviceMaster.Make,(CASE WHEN (SELECT TOP 1 DATEDIFF(MINUTE,T.DeviceDateTime,getdate()) AS LastActMinutes FROM "
                            + " DeviceData  as T where T.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND T.DeviceDate >= '" + fromdate + "' AND T.DeviceDate <= '" + fromdate + "' order by T.DeviceDateTime desc) > 15 THEN '0' "
                            + " ELSE (select TOP 1 convert(varchar(10),Status) from DeviceData as D where D.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND D.DeviceDate >= '" + fromdate + "' AND D.DeviceDate <= '" + fromdate + "' order by 1 desc) END) as InvStatus FROM    DeviceData RIGHT OUTER JOIN "
                            + " SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN "
                            + " MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN "
                            + " PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate + "' AND DeviceData.DeviceDate <= '" + todate + "'";
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SubDeviceName,SubDeviceMaster.SerialNo,MainDeviceMaster.Make,SubDeviceMaster.SubDeviceId,SubDeviceMaster.PerformsOfPlantUniteVolume ";
                QueryDisctionery.OrderPart = @"ORDER BY SubDeviceMaster.SubDeviceName ";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.InverterDateTable>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.InverterDateTable> Get7DaysTableAPI(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName,SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate.ToString("dd/MMM/yyyy") + "' AND DD.DeviceDate <= '" + todate.ToString("dd/MMM/yyyy") + "') AS Day1  ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName, SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS Day1 ";
                }

                QueryDisctionery.TablePart = @" ,MainDeviceMaster.Make,(CASE WHEN (SELECT TOP 1 DATEDIFF(MINUTE,T.DeviceDateTime,getdate()) AS LastActMinutes FROM "
                            + " DeviceData  as T where T.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND T.DeviceDate >= '" + fromdate.ToString("dd/MMM/yyyy") + "' AND T.DeviceDate <= '" + fromdate.ToString("dd/MMM/yyyy") + "' order by T.DeviceDateTime desc) > 15 THEN '0' "
                            + " ELSE (select TOP 1 convert(varchar(10),Status) from DeviceData as D where D.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND D.DeviceDate >= '" + fromdate.ToString("dd/MMM/yyyy") + "' AND D.DeviceDate <= '" + fromdate.ToString("dd/MMM/yyyy") + "' order by 1 desc) END) as InvStatus FROM    DeviceData RIGHT OUTER JOIN "
                            + " SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN "
                            + " MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN "
                            + " PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate.ToString("dd/MMM/yyyy") + "' AND DeviceData.DeviceDate <= '" + todate.ToString("dd/MMM/yyyy") + "'";
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SubDeviceName,SubDeviceMaster.SerialNo,MainDeviceMaster.Make,SubDeviceMaster.SubDeviceId,SubDeviceMaster.PerformsOfPlantUniteVolume ";
                QueryDisctionery.OrderPart = @"ORDER BY SubDeviceMaster.SubDeviceName ";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.InverterDateTable>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.InverterDateTable> Get7DaysTableExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Excluded)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName,SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate + "' AND DD.DeviceDate <= '" + todate + "') AS Day1  ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName, SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS Day1 ";
                }

                QueryDisctionery.TablePart = @" ,MainDeviceMaster.Make,(CASE WHEN (SELECT TOP 1 DATEDIFF(MINUTE,T.DeviceDateTime,getdate()) AS LastActMinutes FROM "
                            + " DeviceData  as T where T.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND T.DeviceDate >= '" + fromdate + "' AND T.DeviceDate <= '" + fromdate + "' order by T.DeviceDateTime desc) > 15 THEN '0' "
                            + " ELSE (select TOP 1 convert(varchar(10),Status) from DeviceData as D where D.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND D.DeviceDate >= '" + fromdate + "' AND D.DeviceDate <= '" + fromdate + "' order by 1 desc) END) as InvStatus FROM    DeviceData RIGHT OUTER JOIN "
                            + " SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN "
                            + " MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN "
                            + " PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate + "' AND DeviceData.DeviceDate <= '" + todate + "' AND SubDeviceMaster.SerialNo NOT IN "+Excluded;
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SubDeviceName,SubDeviceMaster.SerialNo,MainDeviceMaster.Make,SubDeviceMaster.SubDeviceId,SubDeviceMaster.PerformsOfPlantUniteVolume ";
                QueryDisctionery.OrderPart = @"ORDER BY SubDeviceMaster.SubDeviceName ";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.InverterDateTable>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }

        public List<ENT.InverterDateTable> Get7DaysTableTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Excluded)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            try
            {
                parFields.Clear();
                QueryDisctionery = new COM.TTDictionaryQuery();
                if (isRequiredLastOne)
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName,SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume,
                            (Select MAX(DD.Eac)
                            FROM      DeviceData DD RIGHT OUTER JOIN
                                    SubDeviceMaster SBD ON DD.SubDeviceId = SBD.SubDeviceId RIGHT OUTER JOIN
                                    MainDeviceMaster ON SBD.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN
                                    PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId
                            WHERE SBD.SerialNo = SubDeviceMaster.SerialNo AND DD.DeviceDate >= '" + fromdate + "' AND DD.DeviceDate <= '" + todate + "') AS Day1  ";
                }
                else
                {
                    QueryDisctionery.SelectPart = @"SELECT SubDeviceMaster.SubDeviceName AS DeviceName, SubDeviceMaster.SerialNo,SubDeviceMaster.PerformsOfPlantUniteVolume, Sum(DeviceData.Eac) AS Day1 ";
                }

                QueryDisctionery.TablePart = @" ,MainDeviceMaster.Make,(CASE WHEN (SELECT TOP 1 DATEDIFF(MINUTE,T.DeviceDateTime,getdate()) AS LastActMinutes FROM "
                            + " DeviceData  as T where T.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND T.DeviceDate >= '" + fromdate + "' AND T.DeviceDate <= '" + fromdate + "' order by T.DeviceDateTime desc) > 15 THEN '0' "
                            + " ELSE (select TOP 1 convert(varchar(10),Status) from DeviceData as D where D.SubDeviceId = SubDeviceMaster.SubDeviceId "
                            + " AND D.DeviceDate >= '" + fromdate + "' AND D.DeviceDate <= '" + fromdate + "' order by 1 desc) END) as InvStatus FROM    DeviceData RIGHT OUTER JOIN "
                            + " SubDeviceMaster ON DeviceData.SubDeviceId = SubDeviceMaster.SubDeviceId RIGHT OUTER JOIN "
                            + " MainDeviceMaster ON SubDeviceMaster.DeviceId = MainDeviceMaster.DeviceId RIGHT OUTER JOIN "
                            + " PlantMaster ON MainDeviceMaster.PlantId = PlantMaster.PlantId ";
                QueryDisctionery.ParameterPart = @"WHERE PlantMaster.AspNetUserID ='" + UserID.ToString() + "' AND DeviceData.DeviceDate >= '" + fromdate + "' AND DeviceData.DeviceDate <= '" + todate + "' AND SubDeviceMaster.SerialNo IN " + Excluded;
                QueryDisctionery.GroupPart = @"GROUP BY SubDeviceMaster.SubDeviceName,SubDeviceMaster.SerialNo,MainDeviceMaster.Make,SubDeviceMaster.SubDeviceId,SubDeviceMaster.PerformsOfPlantUniteVolume ";
                QueryDisctionery.OrderPart = @"ORDER BY SubDeviceMaster.SubDeviceName ";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstResult = COM.DBHelper.CopyDataReaderToEntity<ENT.InverterDateTable>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstResult;
        }


    }
}
