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
    public class DeviceDataBAL : IDisposable
    {
        DAL.CRUDOperation objDAL = new DAL.CRUDOperation();
        DAL.DeviceDataDAL clsDAL = new DAL.DeviceDataDAL();
        DAL.MainDeviceMasterDAL clsMainDAL = new DAL.MainDeviceMasterDAL();
        public ENT.DeviceDataSUB Entity = new ENT.DeviceDataSUB();
        List<string> strvalidationResult = new List<string>();
        List<ENT.DeviceDataSUB> lstEntity;

        public List<ENT.LastActivityMinutes> GetPlantActiveMinutes(Guid UserID)
        {
            List<ENT.LastActivityMinutes> lstResult = clsDAL.GetPlantActiveMinutes(UserID);
            return lstResult;
        }
        
        public List<ENT.DeviceDataSUB> GetAll(string SubDeviceId, DateTime FromDate, DateTime ToDate)
        {
            lstEntity = new List<ENT.DeviceDataSUB>();
            lstEntity = clsDAL.GetList(SubDeviceId, FromDate, ToDate);
            return lstEntity;
        }

        public List<ENT.DashboardCards> GetDashboardCards(bool isRequiredToday, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetDashboardCards(isRequiredToday, isRequiredLastOne, UserID);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsExcluded(bool isRequiredToday, bool isRequiredLastOne, Guid UserID, String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetDashboardCardsExcluded(isRequiredToday, isRequiredLastOne, UserID, Exclude);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsTop(bool isRequiredToday, bool isRequiredLastOne, Guid UserID, String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetDashboardCardsTop(isRequiredToday, isRequiredLastOne, UserID, Exclude);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsInverterTable(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetDashboardCardsInverterTable(fromdate, todate, isRequiredLastOne, UserID);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsInverterTableExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetDashboardCardsInverterTableExcluded(fromdate, todate, isRequiredLastOne, UserID, Excluded);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetDashboardCardsInverterTableTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetDashboardCardsInverterTableTop(fromdate, todate, isRequiredLastOne, UserID, Excluded);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartData(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetChartData(fromdate, todate, isRequiredLastOne, UserID);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartDataExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetChartDataExcluded(fromdate, todate, isRequiredLastOne, UserID,Exclude);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartDataTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Exclude)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetChartDataTop(fromdate, todate, isRequiredLastOne, UserID, Exclude);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartAreaData(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetChartAreaData(fromdate, todate, isRequiredLastOne, UserID);
            return lstResult;
        }

        public List<ENT.DashboardCards> GetChartAreaDataExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetChartAreaDataExcluded(fromdate, todate, isRequiredLastOne, UserID,Excluded);
            return lstResult;
        }
        
        public List<ENT.DashboardCards> GetChartAreaDataTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Excluded)
        {
            List<ENT.DashboardCards> lstResult = new List<Framework.Entity.DashboardCards>();
            lstResult = clsDAL.GetChartAreaDataTop(fromdate, todate, isRequiredLastOne, UserID, Excluded);
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergy(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            lstResult = clsDAL.GetDailyEnergy(hdLoginID, fromdate, todate, isRequiredLastOne, SubDeviceID, checkbox6a);
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyAPI(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            lstResult = clsDAL.GetDailyEnergyAPI(hdLoginID, fromdate, todate, isRequiredLastOne, SubDeviceID, checkbox6a);
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyExcludeAPI(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a, String Excluded)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            lstResult = clsDAL.GetDailyEnergyExcludedAPI(hdLoginID, fromdate, todate, isRequiredLastOne, SubDeviceID, checkbox6a, Excluded);
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyExclude(Guid hdLoginID, DateTime fromdate, DateTime todate, bool isRequiredLastOne, string SubDeviceID, string checkbox6a,String Excluded)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            lstResult = clsDAL.GetDailyEnergyExcluded(hdLoginID, fromdate, todate, isRequiredLastOne, SubDeviceID, checkbox6a, Excluded);
            return lstResult;
        }

        public List<ENT.DashboardCardsNew> GetDailyEnergyService(DateTime fromdate, DateTime todate)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<Framework.Entity.DashboardCardsNew>();
            lstResult = clsDAL.InsertDailyAllDataReport(fromdate, todate);
            return lstResult;
        }
        
        public List<ENT.InverterDateTable> Get7DaysTable(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            lstResult = clsDAL.Get7DaysTable(fromdate, todate, isRequiredLastOne, UserID);
            return lstResult;
        }
        
        public List<ENT.InverterDateTable> Get7DaysTableAPI(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            lstResult = clsDAL.Get7DaysTableAPI(fromdate, todate, isRequiredLastOne, UserID);
            return lstResult;
        }
        
        public List<ENT.InverterDateTable> Get7DaysTableExcluded(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID,String Excluded)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            lstResult = clsDAL.Get7DaysTableExcluded(fromdate, todate, isRequiredLastOne, UserID,Excluded);
            return lstResult;
        }

        public List<ENT.InverterDateTable> Get7DaysTableTop(DateTime fromdate, DateTime todate, bool isRequiredLastOne, Guid UserID, String Excluded)
        {
            List<ENT.InverterDateTable> lstResult = new List<Framework.Entity.InverterDateTable>();
            lstResult = clsDAL.Get7DaysTableTop(fromdate, todate, isRequiredLastOne, UserID,Excluded);
            return lstResult;
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
        // ~DeviceDataBAL() {
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
