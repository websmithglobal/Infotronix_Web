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
    public class CityMasterBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.CityMasterDAL clsDAL = new DAL.CityMasterDAL();
        public ENT.CityMasterSUB Entity = new ENT.CityMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.CityMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.CityMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.CityName)) { strvalidationResult.Add("City Name Required!"); }
            return strvalidationResult;
        }

        public List<ENT.CityMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            lstEntity = new List<ENT.CityMasterSUB>();
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
                    Entity = (ENT.CityMasterSUB)obj;
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "CityID");
                    COM.HelperMethod.SetValueInObject(obj, Entity.StateID, "StateID");
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
        public List<ENT.CityMasterSUB> GetAll(string search, string StateID)
        {
            lstEntity = new List<ENT.CityMasterSUB>();
            lstEntity = clsDAL.GetList(search, StateID);
            return lstEntity;
        }
        public object GetByPrimaryKey(ENT.CityMasterSUB Entity)
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
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.CityMasterSUB>(x => x.CityID), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.CityMasterSUB>(x => x.Status), false);
                Entity.CityID = PrimarKey;
                Entity.Status = Status;
                if (objDAL.SaveChanges(dctFields, Entity))
                    blnResult = true;
            }
            catch (Exception) { throw; }
            return blnResult;
        }
        public List<ENT.CityMasterSUB> GetAll(string strFilterValue, int Segment, int Status)
        {
            lstEntity = new List<ENT.CityMasterSUB>();
            lstEntity = clsDAL.GetList(strFilterValue, Segment, Status);
            return lstEntity;
        }

        public List<ENT.CityMasterSUB> GetCityByStateID(Guid CountryID)
        {
            lstEntity = new List<ENT.CityMasterSUB>();
            lstEntity = clsDAL.GetListByStateID(CountryID);
            return lstEntity;
        }
    }
}
