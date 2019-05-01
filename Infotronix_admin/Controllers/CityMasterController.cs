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
    [Authorize]
    public class CityMasterController : Controller
    {
        List<ENT.CityMasterSUB> lstEntity = new List<ENT.CityMasterSUB>();
        BAL.CityMasterBAL objBAL = new BAL.CityMasterBAL();
        ENT.CityMasterSUB Model;

        // GET: CityMaster
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ViewBag.CountryMaster = new BAL.CountryMasterBAL().GetAll(string.Empty, 1, 0);            
            ViewBag.PageHeader = "City Master";
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="Administrator")]//Save Entry
        public JsonResult SaveEntry(ENT.CityMasterSUB model, string CityID)
        {
            try
            {
                List<Guid> dctDuplication = new List<Guid>();
                if (model.EntryMode == COM.MyEnumration.EntryMode.ADD)
                {
                    model.Status = COM.MyEnumration.MyStatus.Active;
                    List<ENT.CityMasterSUB> lstResult = new BAL.CityMasterBAL().CheckDuplicateCombination(dctDuplication, COM.MyEnumration.MasterType.MainDeviceMaster, model.CityName);
                    if (lstResult.Count > 0)
                        throw new Exception("City Name Already Exists.");
                    if (objBAL.Insert(model))
                        GlobalVarible.AddMessage("Record Save Successfully");
                }
                else
                {
                    model.CreatedDateTime = DateTime.Now;
                    model.CityID = new Guid(CityID.Replace("/", ""));
                    if (objBAL.Update(model))
                        GlobalVarible.AddMessage("Record Update Successfully");
                }
            }
            catch (Exception ex)
            { GlobalVarible.AddError(ex.Message); }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]//Get Main Device List
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
            var StateID = Request.Form.Get("StateID").ToString();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 1;
            skip = (skip / pageSize) + 1;
            COM.TTPagination.isPageing = true;
            COM.TTPagination.PageSize = pageSize;
            COM.TTPagination.PageNo = Convert.ToInt64(skip);
            lstEntity = objBAL.GetAll(search.ToString().Replace("\0", string.Empty), StateID);
            COM.ExtendedMethods.SortList(lstEntity, sortColumn, sortColumnDir);
            return Json(new
            {
                draw = draw,
                recordsTotal = lstEntity.Count(),
                recordsFiltered = COM.TTPagination.RecordCount,
                data = lstEntity
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]//Delete Entry
        public JsonResult DeleteEntry(string id)
        {
            GlobalVarible.Clear();
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.CityMasterSUB();
                Model.CityID = new Guid(id);
                if (objBAL.Delete(Model))
                    GlobalVarible.AddMessage("Record Delete Successfully.");
                else
                    GlobalVarible.AddError("Internal Server Error Please Try Again");
            }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]//Update Status
        public JsonResult UpdateStatus(string id)
        {
            GlobalVarible.Clear();
            try
            {
                if (id != null)
                {
                    Model = new Josheph.Framework.Entity.CityMasterSUB();
                    Model.CityID = new Guid(id);
                    Model = (ENT.CityMasterSUB)objBAL.GetByPrimaryKey(Model);
                    if (Model.Status == COM.MyEnumration.MyStatus.Active)
                        if (!objBAL.UpdateStatus(Model.CityID, COM.MyEnumration.MyStatus.DeActive))
                            throw new Exception("Internal Server Error in status update.");

                    if (Model.Status == COM.MyEnumration.MyStatus.DeActive)
                        if (!objBAL.UpdateStatus(Model.CityID, COM.MyEnumration.MyStatus.Active))
                            throw new Exception("Internal Server Error in status update.");
                    GlobalVarible.AddMessage("Status Update Successfully.");
                }
            }
            catch (Exception ex)
            { GlobalVarible.AddError(ex.Message); }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]//Edit Record
        public JsonResult EditRecord(string id)
        {
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.CityMasterSUB();
                Model.UpdatedDateTime = DateTime.Now;
                Model.CityID = new Guid(id);
                Model = (ENT.CityMasterSUB)objBAL.GetByPrimaryKey(Model);
            }
            return Json(new { Model = Model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]//Get City By State
        public JsonResult GetCityByState(Guid StateID)
        {
            lstEntity = new List<Josheph.Framework.Entity.CityMasterSUB>();
            lstEntity = objBAL.GetCityByStateID(StateID);
            return Json(lstEntity, JsonRequestBehavior.AllowGet);
        }
    }
}