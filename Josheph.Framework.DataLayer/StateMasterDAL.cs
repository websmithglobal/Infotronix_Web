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
    public class StateMasterDAL
    {
        #region Declare Common Object
        List<ENT.StateMasterSUB> lstEntity = new List<ENT.StateMasterSUB>();
        ENT.StateMasterSUB objEntity = new ENT.StateMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion
        public StateMasterDAL()
        { parFields.Clear(); }

        public List<ENT.StateMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.MainDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 StateID";
                    QueryDisctionery.TablePart = @"FROM  StateMaster ";
                    QueryDisctionery.ParameterPart += " WHERE StateName ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.StateMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.StateMasterSUB> GetListByCountryID(Guid CountryID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select CountryID,StateID,StateName,CreatedBy,Convert(varchar(10),CreatedDateTime,103) as CreatedDateTime,Status  ";
                QueryDisctionery.TablePart = @"from StateMaster ";
                QueryDisctionery.ParameterPart = " WHERE CountryID='" + CountryID.ToString() + "' and Status = 1";
                QueryDisctionery.OrderPart = " Order By StateName ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.StateMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.StateMasterSUB> GetStateAndCountryByCityID(Guid CityID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select A.StateID,B.CountryID ";
                QueryDisctionery.TablePart = @" from CityMaster as A join StateMaster as B on A.StateID = B.StateID ";
                QueryDisctionery.ParameterPart = " WHERE CityID='" + CityID.ToString() + "' and A.Status = 1 and B.Status = 1";
                QueryDisctionery.OrderPart = " Order By StateName ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.StateMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.StateMasterSUB> GetList(string search, string CountryID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select CountryID,StateID,StateName,CreatedBy,Convert(varchar(10),CreatedDateTime,103) as CreatedDate,Status  ";
                QueryDisctionery.TablePart = @"from StateMaster ";
                QueryDisctionery.OrderPart = " Order By StateName ASC";
                QueryDisctionery.ParameterPart = " where CountryID = '" + CountryID + "'";
                if (!string.IsNullOrWhiteSpace(search))
                    QueryDisctionery.ParameterPart = " and M.StateName like '%" + search + "%'";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.StateMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        public List<ENT.StateMasterSUB> GetList(string strFilterValue, int Segment, int Status)
        {
            try
            {
                parFields.Clear();
                //Add Condition Parameters to dictionery
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select StateID,StateName ";
                QueryDisctionery.TablePart = @"from StateMaster";
                QueryDisctionery.ParameterPart = " ";
                if (!string.IsNullOrWhiteSpace(strFilterValue))
                    QueryDisctionery.ParameterPart = " Where StateName Like '%" + strFilterValue + "%'";
                if (Status != 0)
                    QueryDisctionery.ParameterPart += (QueryDisctionery.ParameterPart.Trim() != "" ? " AND " : " WHERE ") + " Status = " + Status.ToString() + " ";

                QueryDisctionery.OrderPart = " Order By StateName ASC";
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.StateMasterSUB>(dr);
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

