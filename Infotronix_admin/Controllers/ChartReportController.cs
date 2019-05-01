using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;
using BAL = Josheph.Framework.BusinessLayer;

namespace Infotronix_admin.Controllers
{
    public class ChartReportController : Controller
    {
        // GET: ChartReport
        public ActionResult Index()
        {
            ViewBag.PageHeader = "Welcome! " + "";
            return View();
        }

        [HttpPost]

        public JsonResult GetDashboardCards()
        {
            ENT.DashboardCards m_SingleDay = new ENT.DashboardCards();
            ENT.DashboardCards m_Total = new ENT.DashboardCards();
            BAL.ChartReportBAL objDashboard = new BAL.ChartReportBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d";
            if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
            {
                lstResult = objDashboard.GetDashboardCards(true, true);
                m_SingleDay.EAC = lstResult.Sum(x => x.EAC) / 10;
            }
            else
            {
                lstResult = objDashboard.GetDashboardCards(true, false);
                m_SingleDay.EAC = lstResult.Sum(x => x.EAC);
            }

            m_SingleDay.EACString = m_SingleDay.EAC.ToString() + " kWh";

            if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
            {
                lstResult = objDashboard.GetDashboardCards(false, true);
                m_Total.EAC = lstResult.Sum(x => x.EAC) / 10;
            }
            else
            {
                lstResult = objDashboard.GetDashboardCards(false, false);
                m_Total.EAC = lstResult.Sum(x => x.EAC);
            }

            m_Total.EACString = m_Total.EAC.ToString() + " kWh";
            List<ENT.InverterDateTable> lstTemp = new List<ENT.InverterDateTable>();
            List<ENT.InverterDateTable> lstTable = new List<ENT.InverterDateTable>();
            if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
            {
                lstTemp = objDashboard.Get7DaysTable(true);
                foreach (ENT.InverterDateTable el in lstTemp)
                {
                    ENT.InverterDateTable obj = new ENT.InverterDateTable();
                    obj.SerialNo = el.SerialNo;
                    obj.Day1 = el.Day1 / 10;
                    obj.Day2 = el.Day2 / 10;
                    obj.Day3 = el.Day3 / 10;
                    obj.Day4 = el.Day4 / 10;
                    obj.Day5 = el.Day5 / 10;
                    obj.Day6 = el.Day6 / 10;
                    obj.Day7 = el.Day7 / 10;
                    obj.Total7Days = el.Total7Days / 10;
                    obj.DeviceName = el.DeviceName;
                    lstTable.Add(obj);
                }
            }
            else { lstTemp = objDashboard.Get7DaysTable(false); lstTable = lstTemp; }
            objDashboard = null;
            return Json(new { Today = m_SingleDay, Entire = m_Total, lstTable = lstTable, lstResult = lstResult }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBarChartValue()
        {
            string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d";
            BAL.ChartReportBAL objDashboard = new BAL.ChartReportBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            List<ENT.BarChartClass> jsonResult = new List<ENT.BarChartClass>();
            if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
            {
                lstResult = objDashboard.GetChartData(DateTime.Now, DateTime.Now, true);
            }
            else
            {
                lstResult = objDashboard.GetChartData(DateTime.Now, DateTime.Now, false);
            }

            foreach (ENT.DashboardCards el in lstResult)
            {
                ENT.BarChartClass obj = new ENT.BarChartClass();
                obj.label = el.SerialNo;
                if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
                {
                    obj.value = el.EAC / 10;
                }
                else
                {
                    obj.value = el.EAC;
                }
                jsonResult.Add(obj);
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBarAreaChartValue()
        {
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            BAL.ChartReportBAL objDashboard = new BAL.ChartReportBAL();
            string ClientID = "d1b28dda-2cd0-44c8-af8f-b8914624ee5d";
            if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), DateTime.Now, true);
            }
            else
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), DateTime.Now, false);
            }
            List<ENT.BarAreaChartClass> jsonResult = new List<ENT.BarAreaChartClass>();
            foreach (ENT.DashboardCards el in lstResult)
            {
                ENT.BarAreaChartClass obj = new ENT.BarAreaChartClass();
                obj.label = el.SerialNo;
                if (ClientID == "d1b28dda-2cd0-44c8-af8f-b8914624ee5d")
                {
                    obj.value = el.EAC / 10;
                }
                else { obj.value = el.EAC; }
                jsonResult.Add(obj);
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

    }
}