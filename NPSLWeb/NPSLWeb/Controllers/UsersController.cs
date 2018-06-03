using Microsoft.AspNetCore.Mvc;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;

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

            ViewBag.Message = "Add User Detail";

            return PartialView("Addrecord");

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