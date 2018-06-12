using Microsoft.AspNetCore.Mvc;
using NPSLWeb.Helper;
using NPSLCore.Models.DB;
using System.Web;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace NPSLWeb.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public JsonResult GetUserDetails(Users usersModel)
        {
            List<Users> result = new List<Users>();
            try
            {
                var userInfo = string.Format("api/GetUsersValidation?userId={0}&password={1}", usersModel.UserId, usersModel.UserPassword);
                var userInforResult = CustomUtility.GetSingleRecord<Users>(userInfo);
                var menuInfo = string.Format("api/GetUsersMenuModel?roleId={0}", userInforResult.FirstOrDefault().RoleId);
                List<MenuModels> menuInforResult = CustomUtility.GetSingleRecord<MenuModels>(menuInfo);
                HttpContext.Session.SetString("LoginSession", userInforResult.FirstOrDefault().FirstName);
                HttpContext.Session.SetInt32("LoginUserId", userInforResult.FirstOrDefault().UserId);
                ViewData["menuModel"] = menuInforResult;
                HttpContext.Session.SetObjectAsJson("MenuSession", menuInforResult);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(ex.Message.ToString());
        
            }
            return Json(result);
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
