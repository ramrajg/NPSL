using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace NPSLWeb.Controllers
{
    public class ReconsileTemplateMasterController : Controller
    {
        public IActionResult Index()
        {
            var TemplateResult = CustomUtility.GetSingleRecord<ReconsileTemplate>(string.Format("api/GetTemplateById"));
            return View(TemplateResult);
        }
        public ActionResult Addrecord()

        {
            var ExtensionInfoResult = CustomUtility.GetSingleRecord<FileExtension>(string.Format("api/GetFileExtension"));
            var DelimeterValue = CustomUtility.GetSingleRecord<Delimiter>(string.Format("api/GetDelimeterValue"));
            var TemplateGroupValue = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue"));
            ViewBag.Message = "Template Master";
            ViewBag.ExtensionList = new SelectList(ExtensionInfoResult, "FileExtensionId", "FileextensionName").Items;
            ViewBag.DelimeterValueList = new SelectList(DelimeterValue, "DelimiterId", "DelimiterValue").Items;
            ViewBag.TemplateGroupValue = new SelectList(TemplateGroupValue, "TemplateGroupId", "TemplateGroupName").Items;
            return PartialView("TemplateAdd");
        }
        [HttpPost]
        public ActionResult Addrecord(ReconsileTemplate templateDetail)

        {
            bool isSuccessStatusCode = false;
            if (ModelState.IsValid)
            {
                var roleInfoResult = CustomUtility.PostDataOfType("api/SaveReconsileTemplate", templateDetail, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return RedirectToAction("Index", "ReconsileTemplateMaster");

        }
        [HttpPost]
        public void DeleteTemplate(int Id)

        {
            bool isSuccessStatusCode = false;
            if (ModelState.IsValid)
            {
                var delTemplateResult = CustomUtility.PostDataOfType("api/DeleteTemplate", Id, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }

        }


        public ActionResult EditTemplate(int Id)
        {
            var TemplateResult = CustomUtility.GetSingleRecord<ReconsileTemplate>(string.Format("api/GetTemplateById?Id=" + Id));
            var ExtensionInfoResult = CustomUtility.GetSingleRecord<FileExtension>(string.Format("api/GetFileExtension"));
            var DelimeterValue = CustomUtility.GetSingleRecord<Delimiter>(string.Format("api/GetDelimeterValue"));
            var TemplateGroupValue = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue"));
            ViewBag.ExtensionList = new SelectList(ExtensionInfoResult, "FileExtensionId", "FileextensionName").Items;
            ViewBag.DelimeterValueList = new SelectList(DelimeterValue, "DelimiterId", "DelimiterValue").Items;
            ViewBag.TemplateGroupValue = new SelectList(TemplateGroupValue, "TemplateGroupId", "TemplateGroupName").Items;
            return PartialView("TemplateEdit", TemplateResult[0]);
        }
       
        [HttpPost]
        public JsonResult EditTemplateApiCall(ReconsileTemplate templateDetail)
        {
            bool isSuccessStatusCode = false;
            var templateInfoResult = CustomUtility.PostDataOfType("api/UpdateTemplate", templateDetail, out isSuccessStatusCode);
            if (!isSuccessStatusCode)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(templateInfoResult.ToString());
            }
            return Json("Success");
        }

    }
}