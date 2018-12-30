using Microsoft.AspNetCore.Mvc;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using NPSLWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace NPSLWeb.Controllers
{
    public class RoleMasterController : Controller
    {
        public IActionResult Index()
        {
            var menuInfo = string.Format("api/GetMenuModelForRoleMaster?roleId=0");
            List<MenuModelForRoleMaster> menuInforResult = CustomUtility.GetSingleRecord<MenuModelForRoleMaster>(menuInfo);
            HttpContext.Session.SetObjectAsJson("RoleMenuMaster", menuInforResult);
            var RolesResult = CustomUtility.GetSingleRecord<Roles>(string.Format("api/GetRoleById"));
            ViewModelRoleMaster mymodel = new ViewModelRoleMaster();
            mymodel.Roles = RolesResult;
            //mymodel.MenuModel = TemplateGroupResult;
            return View(mymodel);
        }
        public ActionResult Addrecord()
        {
            ViewBag.Message = "Role Master";
            return PartialView("RoleAdd");
        }
        [HttpPost]
        public ActionResult Addrecord(Roles RoleDetail)
        {
            return  null;
        }
        [HttpPost]
        public JsonResult AddrecordAPICall(List<SubMenuRoleId> selectedMenuId)

        {
            bool isSuccessStatusCode = false;
            var templateGroupResult = CustomUtility.PostDataOfType("api/SaveRole", selectedMenuId, out isSuccessStatusCode);
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return Json("Success");

        }
      
        public ActionResult EditRole(int Id)
        {
            var menuInfo = string.Format("api/GetMenuModelForRoleMaster?roleId=" + Id);
            List<MenuModelForRoleMaster> menuInforResult = CustomUtility.GetSingleRecord<MenuModelForRoleMaster>(menuInfo);
            HttpContext.Session.SetObjectAsJson("RoleMenuMaster", menuInforResult);
            var RoleResult = CustomUtility.GetSingleRecord<Roles>(string.Format("api/GetRoleById?roleId=" + Id));
            return PartialView("RoleEdit", RoleResult[0]);
        }
        [HttpPost]
        public JsonResult EditrecordAPICall(List<SubMenuRoleId> selectedMenuId)
        {
            bool isSuccessStatusCode = false;
            var templateGroupResult = CustomUtility.PostDataOfType("api/UpdateRole", selectedMenuId, out isSuccessStatusCode);
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return Json("Success");

        }

    }
}