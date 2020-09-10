using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BAL = Josheph.Framework.BusinessLayer;
using ENT = Josheph.Framework.Entity;

namespace Infotronix_api.Controllers
{
    public class PlantMasterController : ApiController
    {
        [HttpGet]
        [Authorize]
        [ActionName("GetPlantList")]
        public HttpResponseMessage GetPlantList()
        {
            List<ENT.PlantMasterSUB> lstResult = new List<ENT.PlantMasterSUB>();
            try
            {
                if (User.IsInRole("Administrator"))
                {
                    lstResult = new BAL.PlantMasterBAL().GetPlantList();
                }
                else if (User.IsInRole("Admin"))
                {
                    lstResult = new BAL.PlantMasterBAL().GetPlantListByUser(User.GetLogged_Userid());
                }
                else{
                    using (BAL.PlantMasterBAL obj = new Josheph.Framework.BusinessLayer.PlantMasterBAL())
                    {
                        obj.Entity = new Josheph.Framework.Entity.PlantMasterSUB();
                        obj.Entity.AspNetUserID = User.GetLogged_Userid();
                        obj.Entity = (ENT.PlantMasterSUB)obj.GetByLoginID(obj.Entity);
                        lstResult.Add(obj.Entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { lstResult });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult });
        }

        [HttpPost]
        [Authorize]
        [ActionName("GetDashboardCards")]
        public HttpResponseMessage GetDashboardCards(Guid hdLoginID)
        {
            ENT.DashboardCards m_SingleDay = new ENT.DashboardCards();
            BAL.DeviceDataBAL objDashboard = new Josheph.Framework.BusinessLayer.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            List<ENT.LastActivityMinutes> intResult = new List<ENT.LastActivityMinutes>();
            string lastDateTime = ""; int activeMinutes = 0;
            try
            {
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


                intResult = objDashboard.GetPlantActiveMinutes(hdLoginID);
                if (intResult.Count > 0)
                {
                    lastDateTime = intResult.FirstOrDefault().LastDateTime.GetFormatedDateTime();
                    activeMinutes = intResult.Sum(x => x.LastActMinutes);
                }
                else
                {
                    lastDateTime = ""; activeMinutes = 0;
                }
                objDashboard = null;
            }
            catch (Exception ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Today = m_SingleDay, lstResult = lstResult, errorMessage = ex.Message.ToString() });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { Today = m_SingleDay, lstResult = lstResult, LastDateTime = lastDateTime, ActiveMinutes = activeMinutes });
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetInverterWiseEnergyProducedChart")]
        public HttpResponseMessage GetInverterWiseEnergyProducedChart(Guid hdLoginID, string plantDate, bool isRequiredDevide)
        {
            BAL.DeviceDataBAL objDashboard = new BAL.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            List<ENT.BarChartClass> jsonResult = new List<ENT.BarChartClass>();
            try
            {
                DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E")
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
                    if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
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
            }
            catch (Exception ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { lstResult = jsonResult, errorMessage = ex.Message.ToString() });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult = jsonResult });
        }

        [Authorize]
        [HttpPost]
        [ActionName("GetEnergyProducedPlantLast30DaysChart")]
        public HttpResponseMessage GetEnergyProducedPlantLast30DaysChart(Guid hdLoginID, string fromDate)
        {
            BAL.DeviceDataBAL objDashboard = new BAL.DeviceDataBAL();
            List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            List<ENT.BarAreaChartClass> jsonResult = new List<ENT.BarAreaChartClass>();
            try
            {
                DateTime dttm = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E")
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
            }
            catch (Exception ex)
            {
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { lstResult = jsonResult, errorMessage = ex.Message.ToString() });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult = jsonResult });
        }

        [HttpPost]
        [Authorize]
        [ActionName("GetDashboardCardsInverterTable")]
        public HttpResponseMessage GetDashboardCardsInverterTable(Guid hdLoginID, string plantDate, bool isRequiredDevide)
        {
            ENT.DashboardCards m_Total = new ENT.DashboardCards();
            BAL.DeviceDataBAL objDashboard = new BAL.DeviceDataBAL();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //List<ENT.DashboardCards> lstResult = new List<ENT.DashboardCards>();
            //if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E")
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, true, hdLoginID);
            //}
            //else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTableExcluded(dttm, dttm, false, hdLoginID, "('1900763842','1900764245')");

            //    List<ENT.DashboardCards> lstResultTop = new List<ENT.DashboardCards>();
            //    lstResultTop = objDashboard.GetDashboardCardsInverterTableTop(dttm, dttm, true, hdLoginID, "('1900763842','1900764245')");

            //    lstResult = lstResult.Concat(lstResultTop).ToList<ENT.DashboardCards>();
            //}
            //else
            //{
            //    lstResult = objDashboard.GetDashboardCardsInverterTable(dttm, dttm, false, hdLoginID);
            //}

            List<ENT.InverterDateTable> lstTemp = new List<ENT.InverterDateTable>();
            List<ENT.InverterDateTable> lstTable = new List<ENT.InverterDateTable>();

            // if (hdLoginID.ToString().ToUpper().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E")
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstTemp = objDashboard.Get7DaysTableAPI(dttm, dttm, true, hdLoginID);
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
                lstTemp = objDashboard.Get7DaysTableAPI(dttm, dttm, false, hdLoginID);
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

            var intResult = objDashboard.GetPlantActiveMinutes(hdLoginID);

            objDashboard = null;

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new
            {
                //Entire = m_Total,
                lstTable = lstTable,
                //lstResult = lstResult,
                LastDateTime = intResult.FirstOrDefault().LastDateTime.GetFormatedDateTime(),
                ActiveMinutes = intResult.Sum(x => x.LastActMinutes)
            });
        }

        [HttpPost]
        [Authorize]
        [ActionName("GetLineChartForGenrateEnregy")]
        public HttpResponseMessage GetLineChartForGenrateEnregy(Guid hdLoginID, string DeviceIDSearch, string plantDate, string checkbox6a)
        {
            List<ENT.DashboardCardsNew> lstResult = new List<ENT.DashboardCardsNew>();
            BAL.DeviceDataBAL objDashboard = new BAL.DeviceDataBAL();
            DateTime dttm = DateTime.ParseExact(plantDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            // if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E")
            if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
            {
                lstResult = objDashboard.GetDailyEnergyAPI(hdLoginID, dttm, dttm, true, DeviceIDSearch, checkbox6a);
            }
            else if (hdLoginID.ToString().ToUpper() == "654F9AC4-601D-4530-BB54-E667D037B1F3")
            {
                lstResult = objDashboard.GetDailyEnergyExcludeAPI(hdLoginID, dttm, dttm, false, DeviceIDSearch, checkbox6a, "('1900763842','1900764245')");
            }
           else {
                lstResult = objDashboard.GetDailyEnergyAPI(hdLoginID, dttm, dttm, false, DeviceIDSearch, checkbox6a);
            }

            List<ENT.LineChartClass> jsonResult = new List<ENT.LineChartClass>();

            if (DeviceIDSearch == "DIV000000")
            {
                foreach (ENT.DashboardCardsNew el in lstResult)
                {
                    ENT.LineChartClass obj = new ENT.LineChartClass();
                    obj.label = el.SerialNo;
                    if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
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
                    if (hdLoginID.ToString().ToUpper() == "CDADA587-1678-4128-B37D-BA9E2B04E508" || hdLoginID.ToString().ToUpper() == "DBF3D275-0110-4D03-A519-7A777D18020E" || hdLoginID.ToString().ToUpper() == "307F425E-2A71-45DC-A528-0E45AAF510F2" || hdLoginID.ToString().ToUpper() == "E0A6FF76-F989-4CA6-AE90-EC32AAA0D33C" || hdLoginID.ToString().ToUpper() == "EB3123B4-6D53-42C5-BF2A-AEBA11257B51" || hdLoginID.ToString().ToUpper() == "B683B70B-5C1A-4275-96DE-9DCDE9101B3E" || hdLoginID.ToString().ToUpper() == "B29E4E42-17CB-4826-9E68-34A8B7236651" || hdLoginID.ToString().ToUpper() == "85C86141-B7C2-410D-A5C8-ECD7A5F325B9" || hdLoginID.ToString().ToUpper() == "A1B8C6E4-D5E4-44EC-8887-5A5227ED5AB4" || hdLoginID.ToString().ToUpper() == "36A5DCF4-A23D-404B-858C-65CBCBAEADE7" || hdLoginID.ToString().ToUpper() == "B4E5DB34-ADE6-4F0F-B5D1-4E0080479721" || hdLoginID.ToString().ToUpper() == "E8B87944-C9F9-4993-9655-830A6ECB4131" || hdLoginID.ToString().ToUpper() == "8C025B11-582B-4039-B923-CA343ECF01E7")
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
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult = jsonResult });
        }

    }
}
