using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;

namespace Josheph.Framework.DataLayer
{
    public class PlantDeviceLastStatus
    {
        #region Declare Common Object
        List<ENT.PlantDeviceLastStatus> lstEntity = new List<ENT.PlantDeviceLastStatus>();
        ENT.PlantDeviceLastStatus objEntity = new ENT.PlantDeviceLastStatus();
        COM.TTDictionary parFields = new COM.TTDictionary();
        COM.DBHelper objDBHelper = new COM.DBHelper();
        COM.TTDictionaryQuery QueryDisctionery = new COM.TTDictionaryQuery();
        #endregion

        public PlantDeviceLastStatus()
        {
            parFields.Clear();
        }

        public List<ENT.PlantDeviceLastStatus> GetList()
        {
            try
            {
                parFields.Clear();

                //Add Condition Parameters to dictionery
                //parFields.Add(COM.HelperMethod.PropertyName<ENT.UserProfile>(x => x.usr_id), userid, COM.Enumration.Operators.WHERE);

                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select * ";
                QueryDisctionery.TablePart = @"from PlantDeviceLastStatus ";

                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantDeviceLastStatus>(dr);
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

        public List<ENT.PlantDeviceLastStatus> GetByDevice(Guid id, int devicetype)
        {
            try
            {
                parFields.Clear();
                
                //Add Query in to string builder object
                QueryDisctionery.SelectPart = "select * ";
                QueryDisctionery.TablePart = @"from PlantDeviceLastStatus ";

                QueryDisctionery.ParameterPart = "Where laststatus_deviceid='"+id+ "' AND laststatus_type="+devicetype;

                //Execute Query and get SQLDataReader
                using (SqlDataReader dr = objDBHelper.ExecuteReaderQuery(QueryDisctionery, parFields, objEntity))
                {
                    lstEntity = COM.DBHelper.CopyDataReaderToEntity<ENT.PlantDeviceLastStatus>(dr);
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
