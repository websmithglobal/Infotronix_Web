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
    public class SubDeviceMasterDAL
    {
        #region Declare Common Object
        List<ENT.SubDeviceMasterSUB> lstEntity = new List<ENT.SubDeviceMasterSUB>();
        ENT.SubDeviceMasterSUB objEntity = new ENT.SubDeviceMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion

        public SubDeviceMasterDAL()
        {
            parFields.Clear();
        }
        public List<ENT.SubDeviceMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {

                if (mstType == COM.MyEnumration.MasterType.SubDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 SubDeviceId";
                    QueryDisctionery.TablePart = @"FROM  SubDeviceMaster ";
                    QueryDisctionery.ParameterPart += " WHERE SubDeviceName ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.SubDeviceMasterSUB>(dr);
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
        public List<ENT.SubDeviceMasterSUB> CheckDuplicateSERIALNO(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string SerialNo)
        {
            try
            {

                if (mstType == COM.MyEnumration.MasterType.SubDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 SubDeviceId";
                    QueryDisctionery.TablePart = @"FROM  SubDeviceMaster ";
                    QueryDisctionery.ParameterPart += " WHERE SerialNo='" + SerialNo + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.SubDeviceMasterSUB>(dr);
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

        public List<ENT.SubDeviceMasterSUB> GetList(string search)
        {
            try
            {
                parFields.Clear();

                QueryDisctionery.SelectPart = "select S.PlantID,S.SubDeviceId,M.DeviceName,(case when S.SubDeviceType=1 then 'Meter' when S.SubDeviceType=2 then 'Inverter'end) as SubDeviceTypeName,S.Status,S.SubDeviceName,S.SerialNo,S.Location,S.InstallDate,S.Make,S.Address,S.ipAddress,S.FTPFolder,S.MultiplyConversation,S.PerformsOfPlantUniteVolume";
                QueryDisctionery.TablePart = @"from SubDeviceMaster as S left join MainDeviceMaster as M on(S.DeviceId=M.DeviceId)";
                QueryDisctionery.OrderPart = " order by S.SubDeviceName desc";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    QueryDisctionery.ParameterPart = "where M.DeviceName like '%" + search + "%' OR  S.SubDeviceName like  '%" + search + "%'";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.SubDeviceMasterSUB>(dr);
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


        public List<ENT.SubDeviceMasterSUB> GetListByDeviceID(Guid DeviceId)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select SubDeviceId,SubDeviceName";
                QueryDisctionery.TablePart = @"from SubDeviceMaster";
                QueryDisctionery.ParameterPart = " WHERE DeviceID='" + DeviceId.ToString() + "' and Status = 1";
                QueryDisctionery.OrderPart = " Order By SubDeviceName ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.SubDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.SubDeviceMasterSUB> GetListByDeviceID(Guid DeviceId, bool Getall)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select SubDeviceId,SubDeviceName";
                QueryDisctionery.TablePart = @"from SubDeviceMaster";
                if (Getall)
                {
                    QueryDisctionery.ParameterPart = " WHERE DeviceID='" + DeviceId.ToString() + "' ";

                }
                else { QueryDisctionery.ParameterPart = " WHERE DeviceID='" + DeviceId.ToString() + "' and Status = 1"; }
                
                QueryDisctionery.OrderPart = " Order By SubDeviceName ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.SubDeviceMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public Decimal GetLastStatus(Guid DeviceId)
        {
            Decimal lststs = -1;
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select top 1 * ";
                QueryDisctionery.TablePart = @"from DeviceData";
                QueryDisctionery.ParameterPart = " WHERE SubDeviceId='" + DeviceId.ToString() + "'";
                QueryDisctionery.OrderPart = " Order By CreatedDate DESC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    ENT.DeviceDataSUB ddEntity = COM.DBHelper.CopyDataReaderToSingleEntity<ENT.DeviceDataSUB>(dr);
                    objDBHelper.Disposed();
                    lststs = Convert.ToDecimal(ddEntity.Status);
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lststs;
        }
    }
}
