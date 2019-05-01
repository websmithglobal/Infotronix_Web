using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENT = Josheph.Framework.Entity;
using COM = Josheph.Framework.Common;
using BAL = Josheph.Framework.BusinessLayer;
using Infotronix_admin.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Infotronix_admin.Controllers
{
    [Authorize]
    public class PlantMasterController : Controller
    {
        List<ENT.PlantMasterSUB> lstEntity = new List<ENT.PlantMasterSUB>();
        BAL.PlantMasterBAL objBAL = new BAL.PlantMasterBAL();
        ENT.PlantMasterSUB Model;

        // GET: PlantMaster
        public ActionResult Index()
        {
            ViewBag.CountryMaster = new BAL.CountryMasterBAL().GetAll(string.Empty, 1, 1);
            ViewBag.PageHeader = "Plant Master";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetPlantReport(string id)
        {
            Model = new Josheph.Framework.Entity.PlantMasterSUB();
            Model.PlantId = Guid.Parse(id);
            Model = (ENT.PlantMasterSUB)objBAL.GetByPrimaryKey(Model);
            return View(Model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AdminPlantMapping(string id)
        {
            if(id != null)
            {

            }

            return View();
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set
            { _userManager = value; }
        }

        [HttpPost]
        [Authorize]//Save Entry
        public JsonResult SaveEntry(ENT.PlantMasterSUB model, string PlantId)
        {
            try
            {
                List<Guid> dctDuplication = new List<Guid>();
                if (model.InstallationSize.ToString() == "0") throw new Exception("Please Select Installation Size.");
                if (model.InstallationType.ToString() == "0") throw new Exception("Please Select Installation Type.");
                //if (model.CountryID.ToString() == "00000000-0000-0000-0000-000000000000") { throw new Exception("Please Select Country."); }
                //if (model.StateID.ToString() == "00000000-0000-0000-0000-000000000000") { throw new Exception("Please Select State."); }
                if (model.CityID.ToString() == "00000000-0000-0000-0000-000000000000") throw new Exception("Please Select City.");
                if (model.EntryMode == COM.MyEnumration.EntryMode.ADD)
                {
                    model.Status = COM.MyEnumration.MyStatus.Active;
                    List<ENT.PlantMasterSUB> lstResult = new BAL.PlantMasterBAL().CheckDuplicateCombination(dctDuplication, COM.MyEnumration.MasterType.PlantMaster, model.PlantName, model.ContactPerson, model.Mobile, model.EmailId);
                    if (lstResult.Count > 0) throw new Exception("Plant Name Already Exists.");
                    List<ENT.PlantMasterSUB> lstResult2 = new BAL.PlantMasterBAL().CheckDuplicateMobile(dctDuplication, COM.MyEnumration.MasterType.PlantMaster, model.PlantName, model.ContactPerson, model.Mobile, model.EmailId);
                    if (lstResult2.Count > 0) throw new Exception("Mobile No Already Exists.!");
                    List<ENT.PlantMasterSUB> lstResult3 = new BAL.PlantMasterBAL().CheckDuplicateEmail(dctDuplication, COM.MyEnumration.MasterType.PlantMaster, model.PlantName, model.ContactPerson, model.Mobile, model.EmailId);
                    if (lstResult3.Count > 0)
                        GlobalVarible.AddError("Email Id Already Exists.!");

                    var user = new ApplicationUser { UserName = model.EmailId, Email = model.EmailId };
                    IdentityResult result = UserManager.Create(user, model.Password);
                    if (result.Succeeded)
                    {
                        this.UserManager.AddToRoleAsync(user.Id, "Client");
                        model.CreatedBy = User.GetLogged_Userid();
                        model.AspNetUserID = Guid.Parse(user.Id);
                        if (objBAL.Insert(model))
                            GlobalVarible.AddMessage("Record Save Successfully");
                    }
                    else
                    {
                        foreach(var e in result.Errors)
                        {
                            GlobalVarible.AddError(e);
                        }
                    }
                }
                else
                {
                    model.CreatedDateTime = DateTime.Now;
                    model.PlantId = new Guid(PlantId.Replace("/", ""));
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
        [Authorize]//Get Plant List
        public JsonResult GetPlantList()
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
            return Json(new { draw = draw, recordsTotal = lstEntity.Count(), recordsFiltered = COM.TTPagination.RecordCount, data = lstEntity }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]//Delete Entry
        public JsonResult DeleteEntry(string id)
        {
            GlobalVarible.Clear();
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.PlantMasterSUB();
                Model.PlantId = new Guid(id);
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
                    Model = new Josheph.Framework.Entity.PlantMasterSUB();
                    Model.PlantId = new Guid(id);
                    Model = (ENT.PlantMasterSUB)objBAL.GetByPrimaryKey(Model);
                    if (Model.Status == COM.MyEnumration.MyStatus.Active)
                        if (!objBAL.UpdateStatus(Model.PlantId, COM.MyEnumration.MyStatus.DeActive)) throw new Exception("Internal Server Error in status update.");

                    if (Model.Status == COM.MyEnumration.MyStatus.DeActive)
                        if (!objBAL.UpdateStatus(Model.PlantId, COM.MyEnumration.MyStatus.Active))
                            throw new Exception("Internal Server Error in status update.");
                    GlobalVarible.AddMessage("Status Update Successfully.");
                }
            }
            catch (Exception ex) { GlobalVarible.AddError(ex.Message); }
            MySession.Current.MessageResult.MessageHtml = GlobalVarible.GetMessageHTML();
            return Json(MySession.Current.MessageResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]//Edit Record
        public JsonResult EditRecord(string id)
        {
            if (id != null)
            {
                Model = new Josheph.Framework.Entity.PlantMasterSUB();
                Model.UpdatedDateTime = DateTime.Now;
                Model.PlantId = new Guid(id);
                Model = (ENT.PlantMasterSUB)objBAL.GetByPrimaryKey(Model);
            }
            return Json(new { Model = Model }, JsonRequestBehavior.AllowGet);
        }
    }
}