using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using System;
using System.Collections.Generic;
using System.Data;

namespace NPSLWeb.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            var userInfo = string.Format("api/GetUserById");
            var userInforResult = CustomUtility.GetSingleRecord<Users>(userInfo);
            return View(userInforResult);
        }
        public ActionResult Edit(int? id)
        {
            return View();
        }
        public ActionResult Addrecord()

        {
            var roleInfo = string.Format("api/GetRoleById");
            var roleInfoResult = CustomUtility.GetSingleRecord<Roles>(roleInfo);
            ViewBag.Message = "Registration";
            ViewBag.RoleList = new SelectList(roleInfoResult, "RoleId", "RoleName").Items;
            return PartialView("UserRegistration");
        }
        [HttpPost]
        public ActionResult Addrecord(Users userDetail)

        {
            bool isSuccessStatusCode = false;
            if (ModelState.IsValid)
            {
                var roleInfoResult = CustomUtility.PostDataOfType("api/SaveUser", userDetail, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return RedirectToAction("Index", "Users");

        }
        [HttpPost]
        public void DeleteUser(int Id)

        {
            bool isSuccessStatusCode = false;
            if (ModelState.IsValid)
            {
                var delUserResult = CustomUtility.PostDataOfType("api/DeleteUser", Id, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }

        }

    }
}