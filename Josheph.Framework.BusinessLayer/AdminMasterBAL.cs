using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Josheph.Framework.Entity;
using DAL = Josheph.Framework.DataLayer;
using COM = Josheph.Framework.Common;

namespace Josheph.Framework.BusinessLayer
{
    public class AdminMasterBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.AdminMasterDAL clsDAL = new DAL.AdminMasterDAL();
        public ENT.AdminMasterSUB Entity = new ENT.AdminMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.AdminMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.AdminMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.DisplayName)) { strvalidationResult.Add("Display Name Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.Email)) { strvalidationResult.Add("User Name Required!"); }            
            return strvalidationResult;
        }
        public List<ENT.AdminMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            lstEntity = new List<ENT.AdminMasterSUB>();
            lstEntity = clsDAL.CheckDuplicate(ParentID, mstType, DeviceName);
            return lstEntity;
        }
        public bool Insert(object obj)
        {
            bool blnResult = false;
            try
            {
                strvalidationResult = ValidationEntry(obj);
                if (strvalidationResult.Count() == 0)
                {
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "AdminID");
                    if (objDAL.Insert(obj))
                        blnResult = true;
                }
                else { throw new Exception(string.Join("<br />", strvalidationResult)); }
            }
            catch (Exception) { throw; }
            return blnResult;
        }

        public bool Update(object obj)
        {
            bool blnResult = false;
            try
            { if (objDAL.Update(obj)) blnResult = true; }
            catch (Exception) { throw; }
            return blnResult;
        }

        public bool Delete(object obj)
        {
            bool blnResult = false;
            try
            { if (objDAL.Delete(obj)) blnResult = true; }
            catch (Exception) { throw; }
            return blnResult;
        }
        public List<ENT.AdminMasterSUB> GetAll(string search)
        {
            lstEntity = new List<ENT.AdminMasterSUB>();
            lstEntity = clsDAL.GetList(search);
            return lstEntity;
        }
        public object GetByPrimaryKey(ENT.AdminMasterSUB Entity)
        {
            object objResult = null;
            try
            {
                DAL.CRUDOperation tt = new DAL.CRUDOperation();
                objResult = tt.GetEntityByPrimartKey(Entity);
            }
            catch (Exception) { throw; }
            return objResult;
        }
        public bool UpdateStatus(Guid PrimarKey, COM.MyEnumration.MyStatus Status)
        {
            bool blnResult = false;
            try
            {
                //Create Fields List in dictionary
                Dictionary<string, bool> dctFields = new Dictionary<string, bool>();
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.AdminMasterSUB>(x => x.AdminID), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.AdminMasterSUB>(x => x.Status), false);
                Entity.AdminID = PrimarKey;
                Entity.Status = Status;
                if (objDAL.SaveChanges(dctFields, Entity))
                    blnResult = true;
            }
            catch (Exception) { throw; }
            return blnResult;
        }

        public List<ENT.AdminMasterSUB> GetAll(string strFilterValue, int Segment, int Status)
        {
            lstEntity = new List<ENT.AdminMasterSUB>();
            lstEntity = clsDAL.GetList(strFilterValue, Segment, Status);
            return lstEntity;
        }

        public List<ENT.AdminMasterSUB> GetAdminUsers()
        {
            lstEntity = new List<ENT.AdminMasterSUB>();
            lstEntity = clsDAL.GetAdminUsers();
            return lstEntity;
        }

        public List<ENT.AspNetUsersSUB> GetUserInfoByName(String UserName)
        {
            List<ENT.AspNetUsersSUB>  lstEntityUser = new List<ENT.AspNetUsersSUB>();
            lstEntityUser = clsDAL.GetUserInfoByName(UserName);
            return lstEntityUser;
        }

    }
}
