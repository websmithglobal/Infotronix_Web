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
    public class OTPCodeMaster 
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        public bool Insert(object obj)
        {
            bool blnResult = false;
            try
            {
                if (obj != null)
                {
                    COM.HelperMethod.SetValueInObject(obj, Guid.NewGuid(), "otp_id");
                    if (objDAL.Insert(obj))
                        blnResult = true;
                }
                else throw new Exception();
            }
            catch (Exception)
            { throw; }
            return blnResult;
        }

        public List<ENT.OTPCodeMaster> GetVerifyOTP(string UserID, string OTPCode)
        {
            List<ENT.OTPCodeMaster> lstEntityOTP = new List<ENT.OTPCodeMaster>();
            lstEntityOTP = new DAL.OTPCodeMaster().GetVerifyOTP(UserID, OTPCode);
            return lstEntityOTP;
        }
    }
}
