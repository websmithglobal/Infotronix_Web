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

namespace Infotronix_admin.Controllers
{
    public class UserAndPlantMappingController : Controller
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

        List<ENT.UserAndPlantMappingSUB> lstEntity = new List<ENT.UserAndPlantMappingSUB>();
        BAL.UserAndPlantMappingBAL objBAL = new BAL.UserAndPlantMappingBAL();
        ENT.UserAndPlantMappingSUB Model;
        // GET: UserAndPlantMapping
        public ActionResult Index()
        {
            ViewBag.PlantMaster = new BAL.PlantMasterBAL().GetAll(string.Empty, 1, 0);
            ViewBag.AdminMaster = new BAL.AdminMasterBAL().GetAdminUsers();
           
            ViewBag.PageHeader = "UserAndPlantMapping";
            return View();
        }

        [HttpPost]
        [Authorize]
        //Get Sub Device List
        public JsonResult GetUserAndPlantMapping()
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
        public JsonResult SaveEntry(ENT.UserAndPlantMappingSUB model, string[] listMultiPlant)
        {
            try
            {
                List<Guid> dctDuplication = new List<Guid>();
                objBAL.DeleteByUserID(model.AspNetUserID);
                foreach (string el in listMultiPlant)
                {
                    model.PlantId = Guid.Parse(el);
                    if (model.EntryMode == COM.MyEnumration.EntryMode.ADD)
                    {
                        if (objBAL.Insert(model))
                            GlobalVarible.AddMessage("Record Save Successfully");
                    }
                    else
                    {
                        model.CreatedDateTime = DateTime.Now;
                        model.UserAndPlantMappingID = new Guid(el.Replace("/", ""));
                        if (objBAL.Update(model))
                            GlobalVarible.AddMessage("Record Update Successfully");
                    }
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
                Guid userid = Guid.Parse(id);
                lstEntity = objBAL.GetListByAspNetUserID(userid);
            }
            return Json(lstEntity, JsonRequestBehavior.AllowGet);
        }
    }
}