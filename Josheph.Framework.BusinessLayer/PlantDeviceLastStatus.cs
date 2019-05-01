using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Josheph.Framework.Entity;
using DAL = Josheph.Framework.DataLayer;
using COM = Josheph.Framework.Common;
using System.Linq.Expressions;

namespace Josheph.Framework.BusinessLayer
{
    public class PlantDeviceLastStatus 
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.PlantDeviceLastStatus clsDAL = new DAL.PlantDeviceLastStatus();
        public ENT.PlantDeviceLastStatus Entity = new ENT.PlantDeviceLastStatus();
        List<ENT.PlantDeviceLastStatus> lstEntity;

        public bool Insert(object obj)
        {
            bool blnResult = false;
            try
            {
                COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "laststatus_id");
                if (objDAL.Insert(obj))
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

        public object GetByPrimaryKey(ENT.PlantDeviceLastStatus Entity)
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

        // this function for just referance for partial update field user have to create seperate function learn from this function.
        public bool UpdatePartial(ENT.PlantDeviceLastStatus objEntity)
        {
            bool blnResult = false;
            try
            {
                //Create Fields List in dictionary
                Dictionary<string, bool> dctFields = new Dictionary<string, bool>();
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.laststatus_id), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.laststatus_deviceid), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.laststatus_status), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.laststatus_type), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.CreatedDateTime), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.SystemDateTime), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantDeviceLastStatus>(x => x.CreatedBy), false);
                objEntity.FieldCollection = dctFields;
                if (objDAL.SaveChanges(objEntity.FieldCollection, objEntity))
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
        
        public List<ENT.PlantDeviceLastStatus> GetAll()
        {
            lstEntity = new List<ENT.PlantDeviceLastStatus>();
            lstEntity = clsDAL.GetList();
            return lstEntity;
        }

        public List<ENT.PlantDeviceLastStatus> GetByDevice(Guid id,int devicetype)
        {
            lstEntity = new List<ENT.PlantDeviceLastStatus>();
            lstEntity = clsDAL.GetByDevice(id, devicetype);
            return lstEntity;
        }
    }
}