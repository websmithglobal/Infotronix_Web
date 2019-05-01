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
    public class CountryMasterDAL
    {
        #region Declare Common Object
        List<ENT.CountryMasterSUB> lstEntity = new List<ENT.CountryMasterSUB>();
        ENT.CountryMasterSUB objEntity = new ENT.CountryMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion
        public CountryMasterDAL()
        { parFields.Clear(); }

        public List<ENT.CountryMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.MainDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 CountryID";
                    QueryDisctionery.TablePart = @"FROM  CountryMaster ";
                    QueryDisctionery.ParameterPart += " WHERE CountryName ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CountryMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }


        public List<ENT.CountryMasterSUB> GetList(string search)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select CountryID,CountryName,CreatedBy,Convert(varchar(10),CreatedDateTime,103) as CreatedDate,Status  ";
                QueryDisctionery.TablePart = @"from CountryMaster ";
                QueryDisctionery.OrderPart = " Order By CountryName ASC";                
                if (!string.IsNullOrWhiteSpace(search))
                    QueryDisctionery.ParameterPart = " where M.CountryName like '%" + search + "%'";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CountryMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        public List<ENT.CountryMasterSUB> GetList(string strFilterValue, int Segment, int Status)
        {
            try
            {
                parFields.Clear();
                //Add Condition Parameters to dictionery
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select CountryID,CountryName ";
                QueryDisctionery.TablePart = @"from CountryMaster";
                QueryDisctionery.ParameterPart = "";
                if (!string.IsNullOrWhiteSpace(strFilterValue))
                    QueryDisctionery.ParameterPart = " and CountryName Like '%" + strFilterValue + "%'";
                if (Status != 0)
                    QueryDisctionery.ParameterPart += (QueryDisctionery.ParameterPart.Trim() != "" ? " AND " : " WHERE ") + " Status = " + Status.ToString() + " ";

                QueryDisctionery.OrderPart = " Order By CountryName ASC";
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.CountryMasterSUB>(dr);
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
