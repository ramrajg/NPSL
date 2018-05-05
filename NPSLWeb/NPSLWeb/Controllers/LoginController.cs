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
                var uriString = string.Format("api/GetUsersValidation?userId={0}&password={1}", usersModel.UserId, usersModel.UserPassword);
                result = CustomUtility.GetSingleRecord<Users>(uriString); 
                HttpContext.Session.SetString("LoginSession",result.FirstOrDefault().FirstName);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(ex.Message.ToString());
        
            }
            return Json(result);
        }
    }
}
