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
    public class MainDeviceMasterDAL
    {
        #region Declare Common Object
        List<ENT.MainDeviceMasterSUB> lstEntity = new List<ENT.MainDeviceMasterSUB>();
        ENT.MainDeviceMasterSUB objEntity = new ENT.MainDeviceMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion

        public MainDeviceMasterDAL()
        {
            parFields.Clear();
        }
        public List<ENT.MainDeviceMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {

                if (mstType == COM.MyEnumration.MasterType.MainDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 DeviceId";
                    QueryDisctionery.TablePart = @"FROM  MainDeviceMaster ";
                    QueryDisctionery.ParameterPart += " WHERE DeviceName ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.MainDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                parFields.Clear();
            }
            return lstEntity;

        }
        public List<ENT.MainDeviceMasterSUB> CheckDuplicateSERIALNO(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string SerialNo)
        {
            try
            {

                if (mstType == COM.MyEnumration.MasterType.PlantMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 DeviceId";
                    QueryDisctionery.TablePart = @"FROM  MainDeviceMaster ";
                    QueryDisctionery.ParameterPart += " WHERE SerialNo='" + SerialNo + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.MainDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                parFields.Clear();
            }
            return lstEntity;

        }

        public List<ENT.MainDeviceMasterSUB> GetList(string search)
        {
            try
            {
                parFields.Clear();

                QueryDisctionery.SelectPart = "select M.DeviceId,(case when M.DeviceType=1 then 'Meter' when M.DeviceType=2 then 'Inverter' when M.DeviceType=3 then 'Smart Logger' end) as DeviceTypeName,M.Status,M.DeviceName,P.PlantName,M.SerialNo,M.Location,M.InstallDate,M.Make,M.Address,M.ipAddress  ";
                QueryDisctionery.TablePart = @"from MainDeviceMaster as M inner join PlantMaster as P on(M.plantid=P.PlantId)";
                QueryDisctionery.OrderPart = " Order By M.DeviceName desc";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    QueryDisctionery.ParameterPart = "where M.DeviceName like '%" + search + "%' or M.SerialNo like '%" + search + "%' or M.Location like '%" + search + "%'";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.MainDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                parFields.Clear();
            }
            return lstEntity;
        }
        public List<ENT.MainDeviceMasterSUB> GetList(string strFilterValue, int Segment, int Status)
        {
            try
            {
                parFields.Clear();
                //Add Condition Parameters to dictionery
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select DeviceId,DeviceName ";
                QueryDisctionery.TablePart = @"from MainDeviceMaster";
                QueryDisctionery.ParameterPart = "";
                if (!string.IsNullOrWhiteSpace(strFilterValue))
                {
                    QueryDisctionery.ParameterPart = " Where DeviceName Like '%" + strFilterValue + "%'";
                }
                if (Status != 0)
                {
                    QueryDisctionery.ParameterPart += (QueryDisctionery.ParameterPart.Trim() != "" ? " AND " : " WHERE ") + " Status = " + Status.ToString() + " ";
                }
                QueryDisctionery.OrderPart = " Order By DeviceName ";
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.MainDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                parFields.Clear();
            }
            return lstEntity;
        }


        public List<ENT.MainDeviceMasterSUB> GetListByPlantID(Guid PlantID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select DeviceId,DeviceName ";
                QueryDisctionery.TablePart = @"from MainDeviceMaster";
                QueryDisctionery.ParameterPart = " WHERE PlantID='" + PlantID.ToString() + "' and Status = 1 ";
                QueryDisctionery.OrderPart = " Order By DeviceName ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.MainDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

    }

}
