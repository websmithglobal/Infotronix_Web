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
    public class StateMasterBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.StateMasterDAL clsDAL = new DAL.StateMasterDAL();
        public ENT.StateMasterSUB Entity = new ENT.StateMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.StateMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.StateMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.StateName)) { strvalidationResult.Add("State Name Required!"); }
            return strvalidationResult;
        }

        public List<ENT.StateMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            lstEntity = new List<ENT.StateMasterSUB>();
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
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "StateID");
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
        public List<ENT.StateMasterSUB> GetAll(string search,string CountryID)
        {
            lstEntity = new List<ENT.StateMasterSUB>();
            lstEntity = clsDAL.GetList(search, CountryID);
            return lstEntity;
        }
        public object GetByPrimaryKey(ENT.StateMasterSUB Entity)
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
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.StateMasterSUB>(x => x.StateID), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.StateMasterSUB>(x => x.Status), false);
                Entity.StateID = PrimarKey;
                Entity.Status = Status;
                if (objDAL.SaveChanges(dctFields, Entity))
                    blnResult = true;
            }
            catch (Exception) { throw; }
            return blnResult;
        }
        public List<ENT.StateMasterSUB> GetAll(string strFilterValue, int Segment, int Status)
        {
            lstEntity = new List<ENT.StateMasterSUB>();
            lstEntity = clsDAL.GetList(strFilterValue, Segment, Status);
            return lstEntity;
        }

        public List<ENT.StateMasterSUB> GetStateByCountryID(Guid CountryID)
        {
            lstEntity = new List<ENT.StateMasterSUB>();
            lstEntity = clsDAL.GetListByCountryID(CountryID);
            return lstEntity;
        }

        public List<ENT.StateMasterSUB> GetStateAndCountryByCityID(Guid CityID)
        {
            lstEntity = new List<ENT.StateMasterSUB>();
            lstEntity = clsDAL.GetStateAndCountryByCityID(CityID);
            return lstEntity;
        }
    }
}
