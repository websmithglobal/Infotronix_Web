using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;

namespace Josheph.Framework.DataLayer
{
    public class MessageSendLog
    {
        #region Declare Common Object
        List<ENT.MessageSendLog> lstEntity = new List<ENT.MessageSendLog>();
        ENT.MessageSendLog objEntity = new ENT.MessageSendLog();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion

        public MessageSendLog()
        {
            parFields.Clear();
        }

        public List<ENT.MessageSendLog> GetList()
        {
            try
            {
                parFields.Clear();

                //Add Condition Parameters to dictionery
                //parFields.Add(COM.HelperMethod.PropertyName<ENT.UserProfile>(x => x.usr_id), userid, COM.Enumration.Operators.WHERE);

                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select * ";
                QueryDisctionery.TablePart = @"from MessageSendLog ";

                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.MessageSendLog>(dr);
                    objDBHelper.Disposed();
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                parFields.Clear();
            }
            return lstEntity;
        }
    }
}
