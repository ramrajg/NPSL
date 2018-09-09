using Microsoft.AspNetCore.Mvc;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using System;
using System.Net;

namespace NPSLWeb.Controllers
{
    public class TemplateGroupController : Controller
    {
        public IActionResult Index()
        {
            var GroupTemplateResult = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue?OnlyActive=0"));
            return View(GroupTemplateResult);
        }
        public ActionResult Addrecord()

        {
            ViewBag.Message = "Template Group Master";
            return PartialView("TemplateGroupAdd");
        }
        [HttpPost]
        public ActionResult Addrecord(TemplateGroup templateDetail)

        {
            bool isSuccessStatusCode = false;
            if (ModelState.IsValid)
            {
                var templateGroupResult = CustomUtility.PostDataOfType("api/SaveTemplateGroup", templateDetail, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return RedirectToAction("Index", "TemplateGroup");

        }
        [HttpPost]
        public JsonResult DeleteTemplate(int Id)

        {
            bool isSuccessStatusCode = false;
            string delTemplateGroupResult = "";
            if (ModelState.IsValid)
            {
                delTemplateGroupResult = CustomUtility.PostDataOfType("api/DeleteTemplateGroup", Id, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(delTemplateGroupResult.ToString());
            }
            return Json("Template Group Deleted Successfully. ");
        }


        public ActionResult EditTemplateGroup(int Id)
        {
            var GroupTemplateResult = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue?Id=" + Id));
            return PartialView("TemplateGroupEdit", GroupTemplateResult[0]);
        }
        [HttpPost]
        public ActionResult EditTemplateGroup(TemplateGroup templateGroupDetail)
        {
            bool isSuccessStatusCode = false;

            var templateGroupResult = CustomUtility.PostDataOfType("api/UpdateTemplateGroup", templateGroupDetail, out isSuccessStatusCode);

            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return RedirectToAction("Index", "TemplateGroup");

        }

    }
}