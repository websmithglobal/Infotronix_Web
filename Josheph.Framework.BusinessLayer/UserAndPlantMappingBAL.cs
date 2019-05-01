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
    public class UserAndPlantMappingBAL
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        List<string> strvalidationResult = new List<string>();
        DAL.UserAndPlantMappingDAL clsDAL = new DAL.UserAndPlantMappingDAL();
        public ENT.UserAndPlantMappingSUB Entity = new ENT.UserAndPlantMappingSUB();
        List<ENT.UserAndPlantMappingSUB> lstEntity;
        private List<string> ValidationEntry(object obj)
        {
            strvalidationResult.Clear();
            Entity = (ENT.UserAndPlantMappingSUB)obj;
            return strvalidationResult;
        }

        public bool Insert(object obj)
        {

            bool blnResult = false;
            try
            {
                strvalidationResult = ValidationEntry(obj);
                if (strvalidationResult.Count() == 0)
                {
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "UserAndPlantMappingID");
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

        public bool DeleteByUserID(Guid Userid)
        {
            bool blnResult = false;
            try
            {
                Entity = new Framework.Entity.UserAndPlantMappingSUB();
                Entity.AspNetUserID = Userid;
                Dictionary<string, bool> dctFields = new Dictionary<string, bool>();
                dctFields.Add("AspNetUserID", true);
                if (objDAL.DeleteByParameter(dctFields, Entity))
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

        public List<ENT.UserAndPlantMappingSUB> GetAll(string search)
        {
            lstEntity = new List<ENT.UserAndPlantMappingSUB>();
            lstEntity = clsDAL.GetList(search);
            return lstEntity;
        }

        public List<ENT.UserAndPlantMappingSUB> GetListByAspNetUserID(Guid UserID)
        {
            lstEntity = new List<ENT.UserAndPlantMappingSUB>();
            lstEntity = clsDAL.GetListByAspNetUserID(UserID);
            return lstEntity;
        }

        



    }
}
