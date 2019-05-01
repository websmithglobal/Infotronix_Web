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
    public class StateMasterController : Controller
    {

        List<ENT.StateMasterSUB> lstEntity = new List<ENT.StateMasterSUB>();
        BAL.StateMasterBAL objBAL = new BAL.StateMasterBAL();
        ENT.StateMasterSUB Model;

        // GET: StateMaster        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ViewBag.CountryMaster = new BAL.CountryMasterBAL().GetAll(string.Empty, 1, 0);
            ViewBag.PageHeader = "State Master";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult SaveEntry(ENT.StateMasterSUB model, string StateID)
        {
            try
            {
                List<Guid> dctDuplication = new List<Guid>();
                if (model.EntryMode == COM.MyEnumration.EntryMode.ADD)
                {
                    model.Status = COM.MyEnumration.MyStatus.Active;
                    List<ENT.StateMasterSUB> lstResult = new BAL.StateMasterBAL().CheckDuplicateCombination(dctDuplication, COM.MyEnumration.MasterType.MainDeviceMaster, model.StateName);
                    if (lstResult.Count > 0)
                        throw new Exception("State Name Already Exists.");
                    if (objBAL.Insert(model))
                        GlobalVarible.AddMessage("Record Save Successfully");
                }
                else
                {
                    model.CreatedDateTime = DateTime.Now;
                    model.StateID = new Guid(StateID.Replace("/", ""));
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
        [Authorize(Roles = "Administrator")]
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
            var CountryID = Request.Form.Get("CountryID").ToString();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 1;
            skip = (skip / pageSize) + 1;
            COM.TTPagination.isPageing = true;
            COM.TTPagination.PageSize = pageSize;
            COM.TTPagination.PageNo = Convert.ToInt64(skip);
            lstEntity = objBAL.GetAll(search.ToString().Replace("\0", string.Empty), CountryID.ToString().Replace("\0", string.Empty));
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
        [Authorize(Roles = "Administrator")]
        public JsonResult DeleteEntry(string id)
        {
            GlobalVarible.Clear();
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.StateMasterSUB();
                Model.StateID = new Guid(id);
                if (objBAL.Delete(Model))
                    GlobalVarible.AddMessage("Record Delete Successfully.");
                else
                    GlobalVarible.AddError("Internal Server Error Please Try Again");
            }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public JsonResult UpdateStatus(string id)
        {
            GlobalVarible.Clear();
            try
            {
                if (id != null)
                {
                    Model = new Josheph.Framework.Entity.StateMasterSUB();
                    Model.StateID = new Guid(id);
                    Model = (ENT.StateMasterSUB)objBAL.GetByPrimaryKey(Model);
                    if (Model.Status == COM.MyEnumration.MyStatus.Active)
                        if (!objBAL.UpdateStatus(Model.StateID, COM.MyEnumration.MyStatus.DeActive))
                            throw new Exception("Internal Server Error in status update.");

                    if (Model.Status == COM.MyEnumration.MyStatus.DeActive)
                        if (!objBAL.UpdateStatus(Model.StateID, COM.MyEnumration.MyStatus.Active))
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
        [Authorize(Roles = "Administrator")]
        public JsonResult EditRecord(string id)
        {
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.StateMasterSUB();
                Model.UpdatedDateTime = DateTime.Now;
                Model.StateID = new Guid(id);
                Model = (ENT.StateMasterSUB)objBAL.GetByPrimaryKey(Model);
            }
            return Json(new { Model = Model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        public JsonResult GetStateByCountry(Guid CountryID)
        {
            lstEntity = new List<Josheph.Framework.Entity.StateMasterSUB>();
            lstEntity = objBAL.GetStateByCountryID(CountryID);
            return Json(lstEntity, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetStateAndCountryByCityID(Guid CityID)
        {
            lstEntity = new List<Josheph.Framework.Entity.StateMasterSUB>();
            lstEntity = objBAL.GetStateAndCountryByCityID(CityID);
            return Json(lstEntity, JsonRequestBehavior.AllowGet);
        }
    }
}