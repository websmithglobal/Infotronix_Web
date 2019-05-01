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
    public class CityMasterDAL
    {
        #region Declare Common Object
        List<ENT.CityMasterSUB> lstEntity = new List<ENT.CityMasterSUB>();
        ENT.CityMasterSUB objEntity = new ENT.CityMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion
        public CityMasterDAL()
        { parFields.Clear(); }

        public List<ENT.CityMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.MainDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 CityID";
                    QueryDisctionery.TablePart = @"FROM  CityMaster ";
                    QueryDisctionery.ParameterPart += " WHERE CityName ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CityMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }


        public List<ENT.CityMasterSUB> GetList(string search, string StateID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select StateID,CityID,CityName,CreatedBy,Convert(varchar(10),CreatedDateTime,103) as CreatedDate,Status";
                QueryDisctionery.TablePart = @"from CityMaster ";
                QueryDisctionery.OrderPart = " Order By CityName ASC";
                QueryDisctionery.ParameterPart = "where StateID = '" + StateID + "'";
                if (!string.IsNullOrWhiteSpace(search))
                    QueryDisctionery.ParameterPart = " and M.CityName like '%" + search + "%'";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CityMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        public List<ENT.CityMasterSUB> GetList(string strFilterValue, int Segment, int Status)
        {
            try
            {
                parFields.Clear();
                //Add Condition Parameters to dictionery
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select CityID,CityName ";
                QueryDisctionery.TablePart = @"from CityMaster";
                QueryDisctionery.ParameterPart = " Order By CityName ASC";
                if (!string.IsNullOrWhiteSpace(strFilterValue))
                    QueryDisctionery.ParameterPart = " Where CityName Like '%" + strFilterValue + "%'";
                if (Status != 0)
                    QueryDisctionery.ParameterPart += (QueryDisctionery.ParameterPart.Trim() != "" ? " AND " : " WHERE ") + " Status = " + Status.ToString() + " ";

                QueryDisctionery.OrderPart = " Order By CityName ";
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CityMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }


        public List<ENT.CityMasterSUB> GetListByStateID(Guid StateID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select StateID,CityID,CityName ";
                QueryDisctionery.TablePart = @"from CityMaster ";
                QueryDisctionery.ParameterPart = " WHERE StateID='" + StateID.ToString() + "' and Status = 1";
                QueryDisctionery.OrderPart = " Order By CityName ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CityMasterSUB>(dr);
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


