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
    public class MessageSendLog : IDisposable
    {

        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.MessageSendLog clsDAL = new DAL.MessageSendLog();
        public ENT.MessageSendLog Entity = new ENT.MessageSendLog();
        List<ENT.MessageSendLog> lstEntity;

        public bool Insert(object obj)
        {
            bool blnResult = false;
            try
            {
                COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "MessageLogId");
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

        public object GetByPrimaryKey(ENT.MessageSendLog Entity)
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
        public bool UpdatePartial(ENT.MessageSendLog objEntity)
        {
            bool blnResult = false;
            try
            {
                //Create Fields List in dictionary
                Dictionary<string, bool> dctFields = new Dictionary<string, bool>();
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MessageSendLog>(x => x.MessageLogId), true);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MessageSendLog>(x => x.Mobile), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MessageSendLog>(x => x.Response), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MessageSendLog>(x => x.IsSent), false);
                dctFields.Add(COM.HelperMethod.PropertyName<ENT.MessageSendLog>(x => x.CreatedDateTime), false);
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

        public List<ENT.MessageSendLog> GetAll()
        {
            lstEntity = new List<ENT.MessageSendLog>();
            lstEntity = clsDAL.GetList();
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
        // ~MessageSendLog() {
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
