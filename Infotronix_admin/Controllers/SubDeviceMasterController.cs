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
    public class SubDeviceMasterController : Controller
    {
        List<ENT.SubDeviceMasterSUB> lstEntity = new List<ENT.SubDeviceMasterSUB>();
        BAL.SubDeviceMasterBAL objBAL = new BAL.SubDeviceMasterBAL();
        ENT.SubDeviceMasterSUB Model;

        // GET: SubDeviceMaster
        public ActionResult Index()
        {
            ViewBag.PlantMaster = new BAL.PlantMasterBAL().GetAll(string.Empty, 1, 1);
            return View();
        }
        [HttpPost]
        [Authorize]
        //Get Sub Device List
        public JsonResult GetSubDeviceList()
        {
            //jQuery DataTables Param
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            try
            {
                //Find paging info
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                //Find order columns info
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var search = Request.Form.Get("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0, skip = start != null ? Convert.ToInt16(start) : 1;
                skip = (skip / pageSize) + 1;
                COM.TTPagination.isPageing = true;
                COM.TTPagination.PageSize = pageSize;
                COM.TTPagination.PageNo = Convert.ToInt64(skip);
                lstEntity = objBAL.GetAll(search.ToString().Replace("\0", string.Empty));
                COM.ExtendedMethods.SortList(lstEntity, sortColumn, sortColumnDir);
            }
            catch (Exception ex)
            { GlobalVarible.AddError(ex.Message); }

            return Json(new
            {
                draw = draw,
                recordsTotal = lstEntity.Count(),
                recordsFiltered = COM.TTPagination.RecordCount,
                data = lstEntity
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        //Save Entry
        public JsonResult SaveEntry(ENT.SubDeviceMasterSUB model, string SubDeviceId)
        {
            try
            {
                List<Guid> dctDuplication = new List<Guid>();
                //if (model.DeviceId.ToString() == "00000000-0000-0000-0000-000000000000") { throw new Exception("Please Select Device Type."); }
                if (model.SubDeviceType.ToString() == "00000000-0000-0000-0000-000000000000") { throw new Exception("Please Select Sub Device Type."); }

                if (model.EntryMode == COM.MyEnumration.EntryMode.ADD)
                {
                    model.Status = COM.MyEnumration.MyStatus.Active;
                    List<ENT.SubDeviceMasterSUB> lstResult = new BAL.SubDeviceMasterBAL().CheckDuplicateCombination(dctDuplication, COM.MyEnumration.MasterType.SubDeviceMaster, model.SubDeviceName);
                    if (lstResult.Count > 0)
                        throw new Exception("Device Name Already Exists.");

                    List<ENT.SubDeviceMasterSUB> lstResult2 = new BAL.SubDeviceMasterBAL().CheckDuplicateSerialNo(dctDuplication, COM.MyEnumration.MasterType.SubDeviceMaster, model.SerialNo);
                    if (lstResult2.Count > 0)
                        throw new Exception("Serial No Already Exists.!");

                    if (objBAL.Insert(model, ""))
                        GlobalVarible.AddMessage("Record Save Successfully");
                }
                else
                {
                    model.CreatedDateTime = DateTime.Now;
                    model.SubDeviceId = new Guid(SubDeviceId.Replace("/", ""));
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
        [Authorize]
        //Get Sub Device By Device ID
        public JsonResult GetSubDeviceByDeviceID(Guid DeviceId)
        {
            lstEntity = new List<Josheph.Framework.Entity.SubDeviceMasterSUB>();
            lstEntity = objBAL.GetSubDeviceByDeviceID(DeviceId, true);
            return Json(lstEntity, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        //Delete Entry
        public JsonResult DeleteEntry(string id)
        {
            GlobalVarible.Clear();
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.SubDeviceMasterSUB();
                Model.SubDeviceId = new Guid(id);
                if (objBAL.Delete(Model))
                    GlobalVarible.AddMessage("Record Delete Successfully.");
                else
                    GlobalVarible.AddError("Internal Server Error Please Try Again");
            }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize]
        //Update Status
        public JsonResult UpdateStatus(string id)
        {
            GlobalVarible.Clear();
            try
            {
                if (id != null)
                {
                    Model = new Josheph.Framework.Entity.SubDeviceMasterSUB();
                    Model.SubDeviceId = new Guid(id);
                    Model = (ENT.SubDeviceMasterSUB)objBAL.GetByPrimaryKey(Model);
                    if (Model.Status == COM.MyEnumration.MyStatus.Active)
                    {
                        if (!objBAL.UpdateStatus(Model.SubDeviceId, COM.MyEnumration.MyStatus.DeActive))
                        {
                            throw new Exception("Internal Server Error in status update.");
                        }
                    }
                    if (Model.Status == COM.MyEnumration.MyStatus.DeActive)
                    {
                        if (!objBAL.UpdateStatus(Model.SubDeviceId, COM.MyEnumration.MyStatus.Active))
                        {
                            throw new Exception("Internal Server Error in status update.");
                        }
                    }
                    GlobalVarible.AddMessage("Status Update Successfully.");
                }
            }
            catch (Exception ex)
            {
                GlobalVarible.AddError(ex.Message);
            }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize]
        //Edit Record
        public JsonResult EditRecord(string id)
        {
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.SubDeviceMasterSUB();
                Model.UpdatedDateTime = DateTime.Now;
                Model.SubDeviceId = new Guid(id);
                Model = (ENT.SubDeviceMasterSUB)objBAL.GetByPrimaryKey(Model);
            }
            return Json(new { Model = Model }, JsonRequestBehavior.AllowGet);
        }
    }
}