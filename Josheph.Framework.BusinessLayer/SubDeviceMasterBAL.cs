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
    public class SubDeviceMasterBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.SubDeviceMasterDAL clsDAL = new DAL.SubDeviceMasterDAL();
        public ENT.SubDeviceMasterSUB Entity = new ENT.SubDeviceMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.SubDeviceMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.SubDeviceMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.SubDeviceName)) { strvalidationResult.Add("Sub Device Name Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.SerialNo)) { strvalidationResult.Add("Serial No Required!"); }
            return strvalidationResult;

        }

        public List<ENT.SubDeviceMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string SubDeviceName)
        {
            lstEntity = new List<ENT.SubDeviceMasterSUB>();
            lstEntity = clsDAL.CheckDuplicate(ParentID, mstType, SubDeviceName);
            return lstEntity;
        }

        public List<ENT.SubDeviceMasterSUB> CheckDuplicateSerialNo(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string SerialNo)
        {
            lstEntity = new List<ENT.SubDeviceMasterSUB>();
            lstEntity = clsDAL.CheckDuplicateSERIALNO(ParentID, mstType, SerialNo);
            return lstEntity;
        }

        public bool Insert(object obj, string Status)
        {

            bool blnResult = false;
            try
            {
                strvalidationResult = ValidationEntry(obj);
                if (strvalidationResult.Count() == 0)
                {
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "SubDeviceId");
                    if (objDAL.Insert(obj))
                    {
                        blnResult = true;
                    }

                }
                else { throw new Exception(string.Join("<br />", strvalidationResult)); }
            }
            catch (Exception)
            {
                throw;
            }
            return blnResult;
        }

        public bool Update(object obj)
        {
            bool blnResult = false;
            try
            {
                if (objDAL.Update(obj))
                {
                    blnResult = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return blnResult;
        }

        public bool Delete(object obj)
        {
            bool blnResult = false;
            try
            {
                if (objDAL.Delete(obj))
                {
                    blnResult = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return blnResult;
        }

        public List<ENT.SubDeviceMasterSUB> GetAll(string search)
        {
            lstEntity = new List<ENT.SubDeviceMasterSUB>();
            lstEntity = clsDAL.GetList(search);
            return lstEntity;
        }

        public object GetByPrimaryKey(ENT.SubDeviceMasterSUB Entity)
        {
            object objResult = null;
            try
            {
                DAL.CRUDOperation tt = new DAL.CRUDOperation();
                objResult = tt.GetEntityByPrimartKey(Entity);
            }
            catch (Exception)
            {
                throw;
            }
            return objResult;
        }

        public bool UpdateStatus(Guid PrimarKey, COM.MyEnumration.MyStatus Status)
        {
            bool blnResult = false;
            try
            {
                //Create Fields List in dictionary
                Dictionary<string, bool> dctFields = new Dictionary<string, bool>();
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.SubDeviceMasterSUB>(x => x.SubDeviceId), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.SubDeviceMasterSUB>(x => x.Status), false);
                Entity.SubDeviceId = PrimarKey;
                Entity.Status = Status;
                if (objDAL.SaveChanges(dctFields, Entity))
                {
                    blnResult = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return blnResult;
        }
        
        public List<ENT.SubDeviceMasterSUB> GetSubDeviceByDeviceID(Guid DeviceId)
        {
            lstEntity = new List<ENT.SubDeviceMasterSUB>();
            lstEntity = clsDAL.GetListByDeviceID(DeviceId);
            return lstEntity;
        }

        public List<ENT.SubDeviceMasterSUB> GetSubDeviceByDeviceID(Guid DeviceId,bool Getall)
        {
            lstEntity = new List<ENT.SubDeviceMasterSUB>();
            lstEntity = clsDAL.GetListByDeviceID(DeviceId,Getall);
            return lstEntity;
        }

        public Decimal GetLastStatus(Guid SubdeviceId)
        {
            Decimal lastSts = -1;
            lastSts = clsDAL.GetLastStatus(SubdeviceId);
            return lastSts;
        }
    }
}
