using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COM = Josheph.Framework.Common;
using ENT = Josheph.Framework.Entity;

namespace Josheph.Framework.DataLayer
{
    public class OTPCodeMaster
    {
        #region Declare Common Object
        List<ENT.AdminMasterSUB> lstEntity = new List<ENT.AdminMasterSUB>();
        ENT.AdminMasterSUB objEntity = new ENT.AdminMasterSUB();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion

        public OTPCodeMaster()
        { parFields.Clear(); }

        public List<ENT.OTPCodeMaster> GetVerifyOTP(string UserID, string OTPCode)
        {
            List<ENT.OTPCodeMaster> lstUserInfo = new List<Entity.OTPCodeMaster>();
            try
            {
                parFields.Clear();
                QueryDisctionery.SelectPart = "SELECT * ";
                QueryDisctionery.TablePart = "FROM OTPCodeMaster ";
                QueryDisctionery.ParameterPart = "WHERE DATEDIFF (minute, SystemDateTime,getdate()) <= 45 AND otp_user_id = '" + UserID + "' AND otp_code = " + Convert.ToInt32(OTPCode) + " ";
                QueryDisctionery.OrderPart = " ORDER BY SystemDateTime Desc";

                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstUserInfo = COM.DBHelper.CopyDataReaderToEntity<ENT.OTPCodeMaster>(dr);
                    objDBHelper.Disposed();
                }
            }
            catch (Exception)
            { throw; }
            finally
            { parFields.Clear(); }
            return lstUserInfo;
        }

    }
}
