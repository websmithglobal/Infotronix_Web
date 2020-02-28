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
    public class PlantMasterDAL
    {
        #region Declare Common Object
        List<ENT.PlantMasterSUB> lstEntity = new List<ENT.PlantMasterSUB>();
        ENT.PlantMasterSUB objEntity = new ENT.PlantMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion

        public PlantMasterDAL()
        {
            parFields.Clear();
        }
        public List<ENT.PlantMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string PlantName, string Person, string mobile, string EmailId)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.PlantMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 PlantId";
                    QueryDisctionery.TablePart = @"FROM  PlantMaster ";
                    QueryDisctionery.ParameterPart += " WHERE PlantName ='" + PlantName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        public List<ENT.PlantMasterSUB> CheckDuplicateEMAIL(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string PlantName, string Person, string mobile, string EmailId)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.PlantMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 PlantId";
                    QueryDisctionery.TablePart = @"FROM  PlantMaster ";
                    QueryDisctionery.ParameterPart += " WHERE EmailId='" + EmailId + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        public List<ENT.PlantMasterSUB> CheckDuplicateMOBILE(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string PlantName, string Person, string mobile, string EmailId)
        {
            try
            {

                if (mstType == COM.MyEnumration.MasterType.PlantMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 PlantId";
                    QueryDisctionery.TablePart = @"FROM  PlantMaster ";
                    QueryDisctionery.ParameterPart += " WHERE Mobile ='" + mobile + "'";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception ex)
            { }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetList(string search)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select PlantId,PlantName,AspnetUserId,ContactPerson,Mobile,EmailId,Password,C.CountryID,D.CountryName, B.StateID,StateName,A.CityID,CityName,Address,Logitude,Latitude,(case when InstallationSize=1 then 'KW' when InstallationSize=2 then 'MW' else 'NA' end) as InstallationSize,(case when InstallationType=1 then 'Proof Top' when InstallationType=2 then 'Ground Top' else 'NA' end) as InstallationType,plantDate,A.Status,A.CreatedDateTime,InstllationAngle,InstllationAzimuth";
                QueryDisctionery.TablePart = @"from PlantMaster as A LEFT join CityMaster as B on A.CityID= B.CityID LEFT join StateMaster as C on B.StateID = C.StateID left join CountryMaster as D on C.CountryID = D.CountryID ";
                if (!string.IsNullOrWhiteSpace(search))
                    QueryDisctionery.ParameterPart = "Where  PlantName like '%" + search + "%' or ContactPerson like '%" + search + "%'";

                QueryDisctionery.OrderPart = " Order By PlantId desc ";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception ex)
            { }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetList(string strFilterValue, int Segment, int Status)
        {
            try
            {
                parFields.Clear();
                //Add Condition Parameters to dictionery
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select PlantId,PlantName,AspNetUserID ";
                QueryDisctionery.TablePart = @"from PlantMaster ";
                QueryDisctionery.ParameterPart = "";

                if (!string.IsNullOrWhiteSpace(strFilterValue))
                    QueryDisctionery.ParameterPart = " Where PlantName Like '%" + strFilterValue + "%'";
                if (Status != 0)
                    QueryDisctionery.ParameterPart += (QueryDisctionery.ParameterPart.Trim() != "" ? " AND " : " WHERE ") + " Status = " + Status.ToString() + " ";

                QueryDisctionery.OrderPart = " Order By PlantName ";
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception ex)
            { }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetAllForAdmin(Guid UserID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select PlantId,PlantName,ContactPerson,Mobile,EmailId,Password,C.CountryID,D.CountryName, B.StateID,StateName,A.CityID,CityName,Address,Logitude,Latitude,(case when InstallationSize=1 then 'KW' when InstallationSize=2 then 'MW' else 'NA' end) as InstallationSize,(case when InstallationType=1 then 'Proof Top' when InstallationType=2 then 'Ground Top' else 'NA' end) as InstallationType,plantDate,A.Status,A.CreatedDateTime,InstllationAngle,InstllationAzimuth";
                QueryDisctionery.TablePart = @"from PlantMaster as A LEFT join CityMaster as B on A.CityID= B.CityID LEFT join StateMaster as C on B.StateID = C.StateID left join CountryMaster as D on C.CountryID = D.CountryID ";
                QueryDisctionery.ParameterPart = " WHERE AspNetUserID ='" + UserID + "'";
                QueryDisctionery.OrderPart = " Order By PlantName";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception ex)
            { }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        
        public List<ENT.PlantMasterSUB> GetPlantList()
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select PlantId,PlantName,AspnetUserId";
                QueryDisctionery.TablePart = @"from PlantMaster ";
                QueryDisctionery.OrderPart = " Order By PlantName";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetPlantListByUser(Guid userID)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select  PlantMaster.PlantId,PlantMaster.PlantName,PlantMaster.AspnetUserId ";
                QueryDisctionery.TablePart = @"from PlantMaster 
                inner join UserAndPlantMapping on UserAndPlantMapping.PlantId = PlantMaster.PlantId";
                QueryDisctionery.ParameterPart = "Where UserAndPlantMapping.AspNetUserID = '" + userID+"'";
                QueryDisctionery.OrderPart = " Order By PlantName";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantMasterSUB>(dr);
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
