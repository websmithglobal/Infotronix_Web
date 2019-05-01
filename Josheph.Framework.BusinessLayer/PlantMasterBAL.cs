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
    public class PlantMasterBAL : IDisposable
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.PlantMasterDAL clsDAL = new DAL.PlantMasterDAL();
        public ENT.PlantMasterSUB Entity = new ENT.PlantMasterSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.PlantMasterSUB> lstEntity;

        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            double n; double Lo, La;
            Entity = (ENT.PlantMasterSUB)obj;
            if (string.IsNullOrWhiteSpace(Entity.PlantName)) strvalidationResult.Add("Plant Name Required!");
            if (string.IsNullOrWhiteSpace(Entity.ContactPerson)) strvalidationResult.Add("ContactPerson Name Required!");
            if (!double.TryParse(Entity.Mobile, out n)) strvalidationResult.Add("Invalid Mobile Number!");
            if (!double.TryParse(Entity.Logitude, out Lo)) strvalidationResult.Add("Invalid Logitude Number!");
            if (!double.TryParse(Entity.Latitude, out La)) strvalidationResult.Add("Invalid Latitude Number!");

            if (string.IsNullOrWhiteSpace(Entity.EmailId)) { strvalidationResult.Add("Email Id  Required!"); }
            else
            {
                bool isValidEmailid = COM.ExtendedMethods.ValidEmail(Entity.EmailId);
                if (isValidEmailid == false)
                {
                    strvalidationResult.Add("Invalid Email Id");
                }
            };

            //if (string.IsNullOrWhiteSpace(Entity.StateID)) { strvalidationResult.Add("State Name Required!"); }
            //if (string.IsNullOrWhiteSpace(Entity.CityID)) { strvalidationResult.Add("City Name Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.Password)) { strvalidationResult.Add("Password Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.InstllationAngle)) { strvalidationResult.Add("InstllationAngle Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.InstllationAzimuth)) { strvalidationResult.Add("InstllationAzimuth Required!"); }
            if (string.IsNullOrWhiteSpace(Entity.plantDate)) { strvalidationResult.Add("Plant Date Required!"); }
            return strvalidationResult;
        }

        public List<ENT.PlantMasterSUB> CheckDuplicateCombination(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string PlantName, string Person, string Mobile, string Email)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.CheckDuplicate(ParentID, mstType, PlantName, Person, Mobile, Email);
            return lstEntity;
        }
        public List<ENT.PlantMasterSUB> CheckDuplicateMobile(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string PlantName, string Person, string Mobile, string Email)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.CheckDuplicateMOBILE(ParentID, mstType, PlantName, Person, Mobile, Email);
            return lstEntity;
        }
        public List<ENT.PlantMasterSUB> CheckDuplicateEmail(List<Guid> ParentID, COM.MyEnumration.MasterType mstType, string PlantName, string Person, string Mobile, string Email)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.CheckDuplicateEMAIL(ParentID, mstType, PlantName, Person, Mobile, Email);
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
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "PlantId");
                    if (objDAL.Insert(obj))
                        blnResult = true;
                }
                else throw new Exception(string.Join("<br />", strvalidationResult));
            }
            catch (Exception)
            { throw; }
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

        public object GetByPrimaryKey(ENT.PlantMasterSUB Entity)
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

        public object GetByLoginID(ENT.PlantMasterSUB Entity)
        {
            object objResult = null;
            try
            {
                DAL.CRUDOperation tt = new DAL.CRUDOperation();
                Dictionary<string, COM.MyEnumration.Operation> fields = new Dictionary<string, COM.MyEnumration.Operation>();
                fields.Add("AspNetUserID", COM.MyEnumration.Operation.WHERE);
                objResult = tt.GetEntityByPerameters(Entity,fields);
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
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantMasterSUB>(x => x.PlantId), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.PlantMasterSUB>(x => x.Status), false);
                Entity.PlantId = PrimarKey;
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


        public List<ENT.PlantMasterSUB> GetAll(string search)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.GetList(search);
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetAll(string strFilterValue, int Segment, int Status)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.GetList(strFilterValue, Segment, Status);
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetAllForAdmin(Guid UserID)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.GetAllForAdmin(UserID);
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetPlantList()
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.GetPlantList();
            return lstEntity;
        }

        public List<ENT.PlantMasterSUB> GetPlantListByUser(Guid userId)
        {
            lstEntity = new List<ENT.PlantMasterSUB>();
            lstEntity = clsDAL.GetPlantListByUser(userId);
            return lstEntity;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PlantMasterBAL() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
