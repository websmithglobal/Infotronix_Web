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
    public class AdminMasterDAL
    {
        #region Declare Common Object
        List<ENT.AdminMasterSUB> lstEntity = new List<ENT.AdminMasterSUB>();
        ENT.AdminMasterSUB objEntity = new ENT.AdminMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion
        public AdminMasterDAL()
        { parFields.Clear(); }

        public List<ENT.AdminMasterSUB> CheckDuplicate(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.MainDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 AdminID";
                    QueryDisctionery.TablePart = @"FROM  AdminMaster ";
                    QueryDisctionery.ParameterPart += " WHERE DisplayName ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.AdminMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.AdminMasterSUB> CheckDuplicateUserName(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            try
            {
                if (mstType == COM.MyEnumration.MasterType.MainDeviceMaster)
                {
                    QueryDisctionery.SelectPart = "SELECT TOP 1 AdminID";
                    QueryDisctionery.TablePart = @"FROM  AdminMaster ";
                    QueryDisctionery.ParameterPart += " WHERE Email ='" + DeviceName + "' ";
                }
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.AdminMasterSUB>(dr);
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

        public List<ENT.AdminMasterSUB> GetList(string search)
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "select AdminID,DisplayName,Email,UserRole,CreatedBy,Convert(varchar(10),CreatedDateTime,103) as CreatedDateTimeText,Status  ";
                QueryDisctionery.TablePart = @"from AdminMaster ";
                QueryDisctionery.OrderPart = " Order By DisplayName ASC";
                if (!string.IsNullOrWhiteSpace(search))
                    QueryDisctionery.ParameterPart = "where DisplayName like '%" + search + "%'";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.AdminMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }
        
        public List<ENT.AdminMasterSUB> GetList(string strFilterValue, int Segment, int Status)
        {
            try
            {
                parFields.Clear();
                //Add Condition Parameters to dictionery
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select AdminID,DisplayName ";
                QueryDisctionery.TablePart = @"from AdminMaster";
                QueryDisctionery.ParameterPart = " ";
                if (!string.IsNullOrWhiteSpace(strFilterValue))
                    QueryDisctionery.ParameterPart = " Where DisplayName Like '%" + strFilterValue + "%'";
                if (Status != 0)
                    QueryDisctionery.ParameterPart += (QueryDisctionery.ParameterPart.Trim() != "" ? " AND " : " WHERE ") + " Status = " + Status.ToString() + " ";

                QueryDisctionery.OrderPart = " Order By DisplayName ASC";
                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.AdminMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.AdminMasterSUB> GetAdminUsers()
        {
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = @"SELECT AspNetUsers.Id AS Email, AspNetUsers.Email AS DisplayName";
                QueryDisctionery.TablePart = @"FROM            AspNetRoles RIGHT OUTER JOIN
                         AspNetUserRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId RIGHT OUTER JOIN
                         AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id ";
                QueryDisctionery.OrderPart = " Order By AspNetUsers.Email ASC";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.AdminMasterSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstEntity;
        }

        public List<ENT.AspNetUsersSUB> GetUserInfoByName(String UserName)
        {
            List<ENT.AspNetUsersSUB> lstUserInfo = new List<Entity.AspNetUsersSUB>();
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "SELECT * ";
                QueryDisctionery.TablePart = "FROM AspNetUsers ";
                QueryDisctionery.ParameterPart = "WHERE UserName = '" + UserName + "' ";
                QueryDisctionery.OrderPart = " ORDER BY UserName";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstUserInfo = COM.DBHelper.CopyDataReaderToEntity<ENT.AspNetUsersSUB>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstUserInfo;
        }

    }
}
