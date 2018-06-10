using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using System.Collections.Generic;

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

            //List<SelectListItem> roleList = new List<SelectListItem>();


            //roleList.Add(new SelectListItem()
            //{
            //    Text = "Manager",
            //            Value = "1",
            //            Selected = true  });
            //roleList.Add(new SelectListItem()
            //{
            //    Text = "Admin",
            //    Value = "2",
            //    Selected = false
            //});
            ViewBag.Message = "Registration";
            //ViewBag.RoleList = roleList;
            ViewBag.RoleList = new SelectList(roleInfoResult, "RoleId", "RoleName").Items;
            return PartialView("UserRegistration");

        }
        [HttpPost]

        public ActionResult Addrecord(Users userDetail)

        {

            Users usr = new Users();

            usr.FirstName = userDetail.FirstName;

            usr.LastName = userDetail.LastName;

            return RedirectToAction("EmployeeDetails", "Employee");

        }
    }
}