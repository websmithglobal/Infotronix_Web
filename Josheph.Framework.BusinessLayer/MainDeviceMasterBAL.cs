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
    public class MainDeviceMasterBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.MainDeviceMasterDAL clsDAL = new DAL.MainDeviceMasterDAL();
        public ENT.MainDeviceMasterSUB Entity = new ENT.MainDeviceMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.MainDeviceMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.MainDeviceMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.DeviceName)) { strvalidationResult.Add("Device Name Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.SerialNo)) { strvalidationResult.Add("Serial No Required!"); }
            return strvalidationResult;

        }
        public List<ENT.MainDeviceMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string DeviceName)
        {
            lstEntity = new List<ENT.MainDeviceMasterSUB>();
            lstEntity = clsDAL.CheckDuplicate(ParentID, mstType, DeviceName);
            return lstEntity;
        }
        public List<ENT.MainDeviceMasterSUB> CheckDuplicateSerialNo(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string SerialNo)
        {
            lstEntity = new List<ENT.MainDeviceMasterSUB>();
            lstEntity = clsDAL.CheckDuplicateSERIALNO(ParentID, mstType, SerialNo);
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
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "DeviceId");
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
        public List<ENT.MainDeviceMasterSUB> GetAll(string search)
        {
            lstEntity = new List<ENT.MainDeviceMasterSUB>();
            lstEntity = clsDAL.GetList(search);
            return lstEntity;
        }
        public object GetByPrimaryKey(ENT.MainDeviceMasterSUB Entity)
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
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MainDeviceMasterSUB>(x => x.DeviceId), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MainDeviceMasterSUB>(x => x.Status), false);
                Entity.DeviceId = PrimarKey;
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
        public List<ENT.MainDeviceMasterSUB> GetAll(string strFilterValue, int Segment, int Status)
        {
            lstEntity = new List<ENT.MainDeviceMasterSUB>();
            lstEntity = clsDAL.GetList(strFilterValue, Segment, Status);
            return lstEntity;
        }

        public List<ENT.MainDeviceMasterSUB> GetDeviceByPlant(Guid PlantID)
        {
            lstEntity = new List<ENT.MainDeviceMasterSUB>();
            lstEntity = clsDAL.GetListByPlantID(PlantID);
            return lstEntity;
        }
    }
}
