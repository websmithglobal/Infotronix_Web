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
    public class UserAndPlantMappingDAL
    {
        List<ENT.UserAndPlantMappingSUB> lstEntity = new List<ENT.UserAndPlantMappingSUB>();
        ENT.UserAndPlantMappingSUB objEntity = new ENT.UserAndPlantMappingSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        public List<ENT.UserAndPlantMappingSUB> GetList(string search)
        {
            try
            {
                parFields.Clear();

                QueryDisctionery.SelectPart = @"SELECT  Distinct  UserAndPlantMapping.AspNetUserID, AspNetUsers.UserName AS DisplayName ";
                QueryDisctionery.TablePart = @"FROM            UserAndPlantMapping LEFT OUTER JOIN
                                                AspNetUsers ON UserAndPlantMapping.AspNetUserID = AspNetUsers.Id";
                QueryDisctionery.OrderPart = " order by AspNetUsers.UserName desc";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    QueryDisctionery.ParameterPart = "where M.DisplayName like '%" + search + "%'";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.UserAndPlantMappingSUB>(dr);
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


        public List<ENT.UserAndPlantMappingSUB> GetListByAspNetUserID(Guid AspNetUserID)
        {
            try
            {
                parFields.Clear();

                QueryDisctionery.SelectPart = @"SELECT        UserAndPlantMapping.PlantId, PlantMaster.PlantName AS DisplayName, PlantMaster.AspNetUserID ";
                QueryDisctionery.TablePart = @"FROM            UserAndPlantMapping LEFT OUTER JOIN PlantMaster ON UserAndPlantMapping.PlantId = PlantMaster.PlantId";
                QueryDisctionery.OrderPart = " order by PlantName Asc";
                QueryDisctionery.ParameterPart = " where UserAndPlantMapping.AspNetUserID = '" + AspNetUserID + "'";
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.UserAndPlantMappingSUB>(dr);
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
    }
}
