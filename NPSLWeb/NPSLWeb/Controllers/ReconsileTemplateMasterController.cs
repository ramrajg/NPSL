using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using System;
using System.Collections.Generic;
using System.Data;

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
            ViewBag.Message = "Template Master";
            ViewBag.ExtensionList = new SelectList(ExtensionInfoResult, "FileExtensionId", "FileextensionName").Items;
            ViewBag.DelimeterValueList = new SelectList(DelimeterValue, "DelimiterId", "DelimiterValue").Items;
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
        //[HttpPost]
        //public void DeleteUser(int Id)

        //{
        //    bool isSuccessStatusCode = false;
        //    if (ModelState.IsValid)
        //    {
        //        var delUserResult = CustomUtility.PostDataOfType("api/DeleteUser", Id, out isSuccessStatusCode);
        //    }
        //    if (!isSuccessStatusCode)
        //    {
        //        //  throw new CustomException(responseString);
        //    }

        //}


        public ActionResult EditTemplate(int Id)
        {
            var TemplateResult = CustomUtility.GetSingleRecord<ReconsileTemplate>(string.Format("api/GetTemplateById?Id=" + Id));
            var ExtensionInfoResult = CustomUtility.GetSingleRecord<FileExtension>(string.Format("api/GetFileExtension"));
            var DelimeterValue = CustomUtility.GetSingleRecord<Delimiter>(string.Format("api/GetDelimeterValue"));
            ViewBag.ExtensionList = new SelectList(ExtensionInfoResult, "FileExtensionId", "FileextensionName").Items;
            ViewBag.DelimeterValueList = new SelectList(DelimeterValue, "DelimiterId", "DelimiterValue").Items;
            return PartialView("TemplateEdit", TemplateResult[0]);
        }
        [HttpPost]
        public ActionResult EditTemplate(ReconsileTemplate templateDetail)
        {
            //bool isSuccessStatusCode = false;

            //var roleInfoResult = CustomUtility.PostDataOfType("api/UpdateUser", userDetail, out isSuccessStatusCode);

            //if (!isSuccessStatusCode)
            //{
            //    //  throw new CustomException(responseString);
            //}
            return RedirectToAction("Index", "ReconsileTemplateMaster");

        }

    }
}