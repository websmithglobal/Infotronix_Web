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
    public class CountryMasterBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.CountryMasterDAL clsDAL = new DAL.CountryMasterDAL();
        public ENT.CountryMasterSUB Entity = new ENT.CountryMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.CountryMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.CountryMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.CountryName)) { strvalidationResult.Add("Country Name Required!"); }
            return strvalidationResult;
        }
        public List<ENT.CountryMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            lstEntity = new List<ENT.CountryMasterSUB>();
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
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "CountryID");
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
        public List<ENT.CountryMasterSUB> GetAll(string search)
        {
            lstEntity = new List<ENT.CountryMasterSUB>();
            lstEntity = clsDAL.GetList(search);
            return lstEntity;
        }
        public object GetByPrimaryKey(ENT.CountryMasterSUB Entity)
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
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.CountryMasterSUB>(x => x.CountryID), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.CountryMasterSUB>(x => x.Status), false);
                Entity.CountryID = PrimarKey;
                Entity.Status = Status;
                if (objDAL.SaveChanges(dctFields, Entity))
                    blnResult = true;
            }
            catch (Exception) { throw; }
            return blnResult;
        }
        public List<ENT.CountryMasterSUB> GetAll(string strFilterValue, int Segment, int Status)
        {
            lstEntity = new List<ENT.CountryMasterSUB>();
            lstEntity = clsDAL.GetList(strFilterValue, Segment, Status);
            return lstEntity;
        }

    }
}
