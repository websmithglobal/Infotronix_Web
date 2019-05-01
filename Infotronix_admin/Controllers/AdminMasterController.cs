using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;
using BAL = Josheph.Framework.BusinessLayer;
using Infotronix_admin.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Infotronix_admin.Controllers
{
    [Authorize]
    public class AdminMasterController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        List<ENT.AdminMasterSUB> lstEntity = new List<ENT.AdminMasterSUB>();
        BAL.AdminMasterBAL objBAL = new BAL.AdminMasterBAL();
        ENT.AdminMasterSUB Model;

        // GET: AdminMaster
        public ActionResult Index()
        {
            ViewBag.PageHeader = "Admin Master";
            return View();
        }
        [HttpPost]
        [Authorize]//Save Entry
        public JsonResult SaveEntry(ENT.AdminMasterSUB model, string AdminID)
        {
            try
            {
                List<Guid> dctDuplication = new List<Guid>();
                if (model.EntryMode == COM.MyEnumration.EntryMode.ADD)
                {
                    model.Status = COM.MyEnumration.MyStatus.Active;
                    List<ENT.AdminMasterSUB> lstResult = new BAL.AdminMasterBAL().CheckDuplicateCombination(dctDuplication, COM.MyEnumration.MasterType.MainDeviceMaster, model.DisplayName);
                    if (lstResult.Count > 0)
                        throw new Exception("Admin Name Already Exists.");
                    if (objBAL.Insert(model))
                        GlobalVarible.AddMessage("Record Save Successfully");
                }
                else
                {
                    model.CreatedDateTime = DateTime.Now;
                    model.AdminID = new Guid(AdminID.Replace("/", ""));
                    if (objBAL.Update(model))
                        GlobalVarible.AddMessage("Record Update Successfully");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, model.UserRole);
                }
            }
            catch (Exception ex)
            { GlobalVarible.AddError(ex.Message); }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
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

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt16(start) : 1;
            skip = (skip / pageSize) + 1;
            COM.TTPagination.isPageing = true;
            COM.TTPagination.PageSize = pageSize;
            COM.TTPagination.PageNo = Convert.ToInt64(skip);
            lstEntity = objBAL.GetAll(search.ToString().Replace("\0", string.Empty));
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
        [Authorize]//Delete Entry
        public JsonResult DeleteEntry(string id)
        {
            GlobalVarible.Clear();
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.AdminMasterSUB();
                Model.AdminID = new Guid(id);
                if (objBAL.Delete(Model))
                    GlobalVarible.AddMessage("Record Delete Successfully.");
                else
                    GlobalVarible.AddError("Internal Server Error Please Try Again");
            }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize]//Update Status
        public JsonResult UpdateStatus(string id)
        {
            GlobalVarible.Clear();
            try
            {
                if (id != null)
                {
                    Model = new Josheph.Framework.Entity.AdminMasterSUB();
                    Model.AdminID = new Guid(id);
                    Model = (ENT.AdminMasterSUB)objBAL.GetByPrimaryKey(Model);
                    if (Model.Status == COM.MyEnumration.MyStatus.Active)
                        if (!objBAL.UpdateStatus(Model.AdminID, COM.MyEnumration.MyStatus.DeActive))
                            throw new Exception("Internal Server Error in status update.");

                    if (Model.Status == COM.MyEnumration.MyStatus.DeActive)
                        if (!objBAL.UpdateStatus(Model.AdminID, COM.MyEnumration.MyStatus.Active))
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
        [Authorize]//Edit Record
        public JsonResult EditRecord(string id)
        {
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.AdminMasterSUB();
                Model.UpdatedDateTime = DateTime.Now;
                Model.AdminID = new Guid(id);
                Model = (ENT.AdminMasterSUB)objBAL.GetByPrimaryKey(Model);
            }
            return Json(new { Model = Model }, JsonRequestBehavior.AllowGet);
        }
    }
}