using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;
using BAL = Josheph.Framework.BusinessLayer;
using System.Globalization;

namespace Infotronix_admin.Controllers
{

    public class DeviceDataController : Controller
    {
        List<ENT.DeviceDataSUB> lstEntity = new List<ENT.DeviceDataSUB>();
        BAL.DeviceDataBAL objBAL = new BAL.DeviceDataBAL();
        ENT.DeviceDataSUB Model;
        // GET: DeviceData
        public ActionResult Index()
        {
            ViewBag.PlantMaster = new BAL.PlantMasterBAL().GetAll(string.Empty, 1, 0);
            ViewBag.PageHeader = "Device Data Report";
            return View();
        }


        [HttpPost]
        [Authorize]//Get Main Device List
        public JsonResult GetMainDeviceList()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //Find paging info
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find order columns info

            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var search = Request.Form.Get("search[value]").FirstOrDefault();
            var SubDeviceID = Request.Form.Get("SubDeviceID").ToString();
            var DeviceDate = Request.Form.Get("DeviceDate").ToString();
            var DeviceFromTime = Request.Form.Get("fromtime").ToString();
            var DeviceToTime = Request.Form.Get("totime").ToString();
            string DDate = string.IsNullOrEmpty(DeviceDate) ? "1900-01-01" : DateTime.ParseExact(DeviceDate.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now;
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (!DateTime.TryParseExact(DeviceDate, "dd/MM/yyyy", provider, DateTimeStyles.None, out FromDate))
                {
                    FromDate = DateTime.Now;
                }

                ToDate = FromDate;
                FromDate = FromDate.AddHours(Convert.ToDouble(DeviceFromTime.Split(':')[0]));
                FromDate = FromDate.AddMinutes(Convert.ToDouble(DeviceFromTime.Split(':')[1]));

                ToDate = ToDate.AddHours(Convert.ToDouble(DeviceToTime.Split(':')[0]));
                ToDate = ToDate.AddMinutes(Convert.ToDouble(DeviceToTime.Split(':')[1]));
            }
            catch
            {
                FromDate = DateTime.Now; FromDate.AddHours(0); FromDate.AddMinutes(0);
                ToDate = DateTime.Now; ToDate.AddHours(0); ToDate.AddMinutes(0);
            }

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 1;
            skip = (skip / pageSize) + 1;
            COM.TTPagination.isPageing = true;
            COM.TTPagination.PageSize = pageSize;
            COM.TTPagination.PageNo = Convert.ToInt64(skip);
            lstEntity = objBAL.GetAll(SubDeviceID, FromDate, ToDate);
            COM.ExtendedMethods.SortList(lstEntity, sortColumn, sortColumnDir);
            return Json(new
            {
                draw = draw,
                recordsTotal = lstEntity.Count(),
                recordsFiltered = COM.TTPagination.RecordCount,
                data = lstEntity
            }, JsonRequestBehavior.AllowGet);
        }



    }
}