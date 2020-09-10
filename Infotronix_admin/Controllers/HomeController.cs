using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BAL = Josheph.Framework.BusinessLayer;
using ENT = Josheph.Framework.Entity;

namespace Infotronix_admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        BAL.DeviceDataBAL objDashboard;

        public ActionResult Index()
        {
            ViewBag.PlantMaster = new BAL.PlantMasterBAL().GetAll(string.Empty, 1, 0);

            using (BAL.PlantMasterBAL obj = new Josheph.Framework.BusinessLayer.PlantMasterBAL())
            {
                obj.Entity = new Josheph.Framework.Entity.PlantMasterSUB();
                obj.Entity.AspNetUserID = User.GetLogged_Userid();
                obj.Entity = (ENT.PlantMasterSUB)obj.GetByLoginID(obj.Entity);
                if (obj.Entity != null)
                    ViewBag.PlantName = obj.Entity.PlantName;
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetDashboardCards(Guid hdLoginID, string plantDate)
        {
            ENT.DashboardCards m_SingleDay = new ENT.DashboardCards();
            ENT.DashboardCards m_Total = new ENT.DashboardCards();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetDashboardCards(true, true, hdLoginID);
                m_SingleDay.EAC = lstResult.Sum(x => x.EAC);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetDashboardCardsExcluded(true, false, hdLoginID, "('1900763842','1900764245')");

                // geting top record
                List<ENT.DashboardCards> lstResultTop = new List<ENT.DashboardCards>();
                lstResultTop = objDashboard.GetDashboardCardsTop(true, true, hdLoginID, "('1900763842','1900764245')");

                lstResult = lstResult.Concat(lstResultTop).ToList<ENT.DashboardCards>();

                m_SingleDay.EAC = lstResult.Sum(x => x.EAC);
            }
            else
            {
                lstResult = objDashboard.GetDashboardCards(true, false, hdLoginID);
                m_SingleDay.EAC = lstResult.Sum(x => x.EAC);
            }

            m_SingleDay.EACString = m_SingleDay.EAC.ToString() + " kWh";

            List<ENT.InverterDateTable> lstTemp = new List<ENT.InverterDateTable>();
            List<ENT.InverterDateTable> lstTable = new List<ENT.InverterDateTable>();

            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508")
            {
                lstTemp = objDashboard.Get7DaysTable(dttm, dttm, true, hdLoginID);
                foreach (ENT.InverterDateTable el in lstTemp)
                {
                    ENT.InverterDateTable obj = new ENT.InverterDateTable();
                    obj.SerialNo = el.SerialNo;
                    obj.Day1 = el.Day1;
                    obj.Day2 = el.Day2;
                    obj.Day3 = el.Day3;
                    obj.Day4 = el.Day4;
                    obj.Day5 = el.Day5;
                    obj.Day6 = el.Day6;
                    obj.Day7 = el.Day7;
                    obj.Total7Days = el.Total7Days;
                    obj.DeviceName = el.DeviceName;
                    lstTable.Add(obj);
                }
            }
            else { lstTemp = objDashboard.Get7DaysTable(dttm, dttm, false, hdLoginID); lstTable = lstTemp; }

            var intResult = objDashboard.GetPlantActiveMinutes(hdLoginID);
            objDashboard = null;

            return Json(new { Today = m_SingleDay, Entire = m_Total, lstTable = lstTable, lstResult = lstResult, LastDateTime = intResult.FirstOrDefault().LastDateTime.GetFormatedDateTime(), ActiveMinutes = intResult.Sum(x => x.LastActMinutes) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetDashboardCardsInverterTable(Guid hdLoginID, string plantDate, bool isRequiredDevide)
        {
            //ENT.DashboardCards m_SingleDay = new ENT.DashboardCards();
            ENT.DashboardCards m_Total = new ENT.DashboardCards();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, true, hdLoginID);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetDashboardCardsInverterTableExcluded(dttm, dttm, false, hdLoginID, "('1900763842','1900764245')");

                List<ENT.DashboardCards> lstResultTop = new List<ENT.DashboardCards>();
                lstResultTop = objDashboard.GetDashboardCardsInverterTableTop(dttm, dttm, true, hdLoginID, "('1900763842','1900764245')");

                lstResult = lstResult.Concat(lstResultTop).ToList<ENT.DashboardCards>();
            }
            else
            {
                lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, false, hdLoginID);
            }

            // m_Total.EACString = m_Total.EAC.ToString() + " kWh";
            List<ENT.InverterDateTable> lstTemp = new List<ENT.InverterDateTable>();
            List<ENT.InverterDateTable> lstTable = new List<ENT.InverterDateTable>();

            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstTemp = objDashboard.Get7DaysTable(dttm, dttm, true, hdLoginID);
            }
            else if (hdLoginID.ToString().ToUpper().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstTemp = objDashboard.Get7DaysTableExcluded(dttm, dttm, false, hdLoginID, "('1900763842','1900764245')");

                List<ENT.InverterDateTable> lstTempTop = new List<ENT.InverterDateTable>();
                lstTempTop = objDashboard.Get7DaysTableTop(dttm, dttm, true, hdLoginID, "('1900763842','1900764245')");

                lstTemp = lstTemp.Concat(lstTempTop).ToList<ENT.InverterDateTable>();
            }
            else
            {
                lstTemp = objDashboard.Get7DaysTable(dttm, dttm, false, hdLoginID);
            }

            foreach (ENT.InverterDateTable el in lstTemp)
            {
                ENT.InverterDateTable obj = new ENT.InverterDateTable();
                obj.SerialNo = el.SerialNo;
                obj.Day1 = el.Day1;
                if (isRequiredDevide)
                {
                    obj.Day1 = decimal.Round((obj.Day1 / el.PerformsOfPlantUniteVolume), 2, MidpointRounding.AwayFromZero);
                }
                obj.Day2 = el.Day2;
                obj.Day3 = el.Day3;
                obj.Day4 = el.Day4;
                obj.Day5 = el.Day5;
                obj.Day6 = el.Day6;
                obj.Day7 = el.Day7;
                obj.Total7Days = el.Total7Days;
                obj.DeviceName = el.DeviceName;
                obj.Make = el.Make;
                obj.InvStatus = el.InvStatus;

                lstTable.Add(obj);
            }

            lstTable = lstTable.OrderBy(x => x.DeviceName).ToList<ENT.InverterDateTable>();

            var intResult = objDashboard.GetPlantActiveMinutes(hdLoginID);

            objDashboard = null;

            return Json(new
            {
                Entire = m_Total,
                lstTable = lstTable,
                lstResult = lstResult,
                LastDateTime = intResult.FirstOrDefault().LastDateTime.GetFormatedDateTime(),
                ActiveMinutes = intResult.Sum(x => x.LastActMinutes)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetBarChartValue(Guid hdLoginID, string plantDate, bool isRequiredDevide)
        {
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            List<ENT.BarChartClass> jsonResult = new List<ENT.BarChartClass>();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetChartData(dttm, dttm, true, hdLoginID);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetChartDataExcluded(dttm, dttm, false, hdLoginID, "('1900763842','1900764245')");

                List<ENT.DashboardCards> lstResultTop = new List<ENT.DashboardCards>();
                lstResultTop = objDashboard.GetChartDataTop(dttm, dttm, true, hdLoginID, "('1900763842','1900764245')");

                lstResult = lstResult.Concat(lstResultTop).ToList<ENT.DashboardCards>();
            }
            else
            {
                lstResult = objDashboard.GetChartData(dttm, dttm, false, hdLoginID);
            }

            foreach (ENT.DashboardCards el in lstResult)
            {
                ENT.BarChartClass obj = new ENT.BarChartClass();
                obj.label = el.SerialNo;
                if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
                {
                    obj.value = el.EAC;
                }
                else
                {
                    obj.value = el.EAC;
                }
                if (isRequiredDevide)
                {
                    obj.value = (obj.value / el.PerformsOfPlantUniteVolume);
                }
                jsonResult.Add(obj);
            }

            jsonResult = jsonResult.OrderBy(x => x.label).ToList<ENT.BarChartClass>();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetBarAreaChartValue(Guid hdLoginID, string fromDate)
        {
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            DateTime dttm = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), dttm, true, hdLoginID);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetChartAreaDataExcluded(DateTime.Now.AddDays(-30), dttm, false, hdLoginID, "('1900763842','1900764245')");

                List<ENT.DashboardCards> lstResultTop = new List<ENT.DashboardCards>();
                lstResultTop = objDashboard.GetChartAreaDataTop(DateTime.Now.AddDays(-30), dttm, true, hdLoginID, "('1900763842','1900764245')");

                // merge all value of same date

                foreach (ENT.DashboardCards itm in lstResult)
                {
                    var i = lstResultTop.Find(x => x.SerialNo == itm.SerialNo);
                    if (i != null)
                    {
                        itm.EAC = itm.EAC + i.EAC;
                    }
                }
            }
            else
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), dttm, false, hdLoginID);
            }
            List<ENT.BarAreaChartClass> jsonResult = new List<ENT.BarAreaChartClass>();
            foreach (ENT.DashboardCards el in lstResult)
            {
                ENT.BarAreaChartClass obj = new ENT.BarAreaChartClass();
                obj.label = el.SerialNo;
                if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
                {
                    obj.value = el.EAC;
                }
                else { obj.value = el.EAC; }
                jsonResult.Add(obj);
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetLineChartForGenrateEnregy(Guid hdLoginID, string DeviceIDSearch, string plantDate, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<ENT.DashboardCardsNew>();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetDailyEnergy(hdLoginID, dttm, dttm, true, DeviceIDSearch, checkbox6a);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetDailyEnergyExclude(hdLoginID, dttm, dttm, false, DeviceIDSearch, checkbox6a, "('1900763842','1900764245')");
            }
            else
            {
                lstResult = objDashboard.GetDailyEnergy(hdLoginID, dttm, dttm, false, DeviceIDSearch, checkbox6a);
            }
            List<ENT.LineChartClass> jsonResult = new List<ENT.LineChartClass>();
            //foreach (ENT.DashboardCardsNew el in lstResult)
            //{
            //    ENT.LineChartClass obj = new ENT.LineChartClass();
            //    obj.label = el.SerialNo;
            //    if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C")
            //    {
            //        if (Convert.ToDecimal(el.EAC) >= 0)
            //        {
            //            obj.value = el.EAC;
            //        }
            //    }
            //    else { obj.value = el.EAC; }
            //    jsonResult.Add(obj);
            //}
            if (DeviceIDSearch == "DIV000000")
            {
                foreach (ENT.DashboardCardsNew el in lstResult)
                {
                    ENT.LineChartClass obj = new ENT.LineChartClass();
                    obj.label = el.SerialNo;
                    if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
                    {
                        obj.value = el.EAC;
                    }
                    else { obj.value = el.EAC; }
                    jsonResult.Add(obj);
                }
            }
            else
            {
                foreach (ENT.DashboardCardsNew el in lstResult)
                {
                    ENT.LineChartClass obj = new ENT.LineChartClass();
                    obj.label = el.SerialNo;
                    if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
                    {
                        if (Convert.ToDecimal(el.EAC) >= 0)
                        {
                            obj.value = el.EAC;
                        }
                    }
                    else { obj.value = el.EAC; }
                    jsonResult.Add(obj);
                }
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetLineChartForGenrateEnregyService(string plantDate)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<ENT.DashboardCardsNew>();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            lstResult = objDashboard.GetDailyEnergyService(dttm, dttm);
            return Json(lstResult, JsonRequestBehavior.AllowGet);
        }

        #region Convert Excel

        public void Convert30daysCSV(Guid hdLoginID, string plantDate)
        {
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), dttm, true, hdLoginID);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), dttm, false, hdLoginID);
            }
            else
            {
                lstResult = objDashboard.GetChartAreaData(DateTime.Now.AddDays(-30), dttm, false, hdLoginID);
            }
            List<ENT.BarAreaChartClass> jsonResult = new List<ENT.BarAreaChartClass>();
            foreach (ENT.DashboardCards el in lstResult)
            {
                ENT.BarAreaChartClass obj = new ENT.BarAreaChartClass();
                obj.label = el.SerialNo;
                if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
                {
                    obj.value = el.EAC;
                }
                else { obj.value = el.EAC; }
                jsonResult.Add(obj);
            }

            DataTable students = ToDataTable(jsonResult);

            DataSetToExcel(students, "plantdata30days.xls");
        }

        public void ConvertCSV(Guid hdLoginID, string plantDate)
        {
            //ENT.DashboardCards m_SingleDay = new ENT.DashboardCards();
            ENT.DashboardCards m_Total = new ENT.DashboardCards();
            objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2")
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, true, hdLoginID);
            //    // m_SingleDay.EAC = lstResult.Sum(x => x.EAC);
            //}
            //else
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, false, hdLoginID);
            //    // m_SingleDay.EAC = lstResult.Sum(x => x.EAC);
            //}

            ////m_SingleDay.EACString = m_SingleDay.EAC.ToString() + " kWh";

            //if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2")
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, true, hdLoginID);
            //    //m_Total.EAC = lstResult.Sum(x => x.EAC);
            //}
            //else
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, false, hdLoginID);
            //    //m_Total.EAC = lstResult.Sum(x => x.EAC);
            //}

            List<ENT.InverterDateTable> lstTemp = new List<ENT.InverterDateTable>();
            List<ENT.InverterDateTable> lstTable = new List<ENT.InverterDateTable>();
            if (hdLoginID.ToString().ToUpper().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508")
            {
                lstTemp = objDashboard.Get7DaysTable(dttm, dttm, true, hdLoginID);
                foreach (ENT.InverterDateTable el in lstTemp)
                {
                    ENT.InverterDateTable obj = new ENT.InverterDateTable();
                    obj.SerialNo = el.SerialNo;
                    obj.Day1 = el.Day1;
                    obj.Day2 = el.Day2;
                    obj.Day3 = el.Day3;
                    obj.Day4 = el.Day4;
                    obj.Day5 = el.Day5;
                    obj.Day6 = el.Day6;
                    obj.Day7 = el.Day7;
                    obj.Total7Days = el.Total7Days;
                    obj.DeviceName = el.DeviceName;
                    lstTable.Add(obj);
                }
            }
            else { lstTemp = objDashboard.Get7DaysTable(dttm, dttm, false, hdLoginID); lstTable = lstTemp; }
            int intResult = objDashboard.GetPlantActiveMinutes(hdLoginID).Sum(x => x.LastActMinutes);
            objDashboard = null;

            DataTable students = ToDataTable(lstTable);
            students.Columns.Remove("Day2");
            students.Columns.Remove("Day3");
            students.Columns.Remove("Day4");
            students.Columns.Remove("Day5");
            students.Columns.Remove("Day6");
            students.Columns.Remove("Day7");
            students.Columns.Remove("Total7Days");
            DataSetToExcel(students, "INVERTERDATA.xls");

            //MemoryStream stream = DataSetToExcel(students);
            //var filename = "ExampleCSV.csv";
            //var contenttype = "text/csv";

            //Response.Clear();
            //Response.ContentType = contenttype;
            //Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "text/csv";
            ////Response.BinaryWrite(stream.ToArray());
            //Response.Write(stream);
            //Response.End();
        }

        public int ConvertExcel(string htmlTable)
        {
            string Filenamepath = "abc.xls";
            string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/Report/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += Filenamepath;
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            if (Filenamepath == string.Empty)
                return 0;
            StreamWriter SWriter = new StreamWriter(path);
            string str = string.Empty;
            //Int32 colspan = table.Columns.Count;
            //str += "<Table border=2><tr>";
            //foreach (DataColumn DBCol in table.Columns)
            //    str += "<TH>" + DBCol.ColumnName + "</TH>";
            //str += "</TR>";
            //foreach (DataRow DBRow in table.Rows)
            //{
            //    str += "<TR>";
            //    foreach (DataColumn DBCol in table.Columns)
            //        str += "<TD>" + Convert.ToString(DBRow[DBCol.ColumnName]) + "</TD>";
            //    str += "</TR>";
            //}
            //str += "</TABLE>";
            SWriter.WriteLine(htmlTable);
            SWriter.Flush();
            SWriter.Close();
            if (Filenamepath.Length > 5)
                return DownloadFile(path);
            else
                return 0;
        }

        public int DataSetToExcel(DataTable table, string Filenamepath)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/Report/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += Filenamepath;

            //String strRequest = Request.QueryString["file"];
            //FileInfo file = new FileInfo(path);
            //if (file.Exists)
            //{
            //    file.Delete();
            //}
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            if (Filenamepath == string.Empty)
                return 0;
            StreamWriter SWriter = new StreamWriter(path);
            string str = string.Empty;
            Int32 colspan = table.Columns.Count;
            str += "<Table border=2><tr>";
            foreach (DataColumn DBCol in table.Columns)
                str += "<TH>" + DBCol.ColumnName + "</TH>";
            str += "</TR>";
            foreach (DataRow DBRow in table.Rows)
            {
                str += "<TR>";
                foreach (DataColumn DBCol in table.Columns)
                    str += "<TD>" + Convert.ToString(DBRow[DBCol.ColumnName]) + "</TD>";
                str += "</TR>";
            }
            str += "</TABLE>";
            SWriter.WriteLine(str);
            SWriter.Flush();
            SWriter.Close();
            if (Filenamepath.Length > 5)
                return DownloadFile(path);
            else
                return 0;
        }

        public int DownloadFile(string FPath)
        {
            String strRequest = Request.QueryString["file"];
            FileInfo file = new FileInfo(FPath);
            if (file.Exists)
            {
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                //Response.AddHeader("Content-Length", file.Length.ToString());
                //Response.ContentType = "application/ms-excel";
                //Response.WriteFile(FPath);
                //Response.End();
                return 1;
            }
            else
            {
                Response.Write("This file does not exist.");
                return 0;
            }

        }

        public DataTable ToDataTable<T>(IList<T> data)// T is any generic type
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item);
                table.Rows.Add(values);
            }
            return table;
        }

        #endregion
    }
}