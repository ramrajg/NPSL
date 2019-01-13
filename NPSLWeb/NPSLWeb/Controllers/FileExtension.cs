using Microsoft.AspNetCore.Mvc;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using System;
using System.Net;

namespace NPSLWeb.Controllers
{
    public class FileExtensionController : Controller
    {
        public IActionResult Index()
        {
            var FileExtensionResult = CustomUtility.GetSingleRecord<FileExtension>(string.Format("api/GetFileExtension"));
            return View(FileExtensionResult);
        }
        public ActionResult Addrecord()

        {
            ViewBag.Message = "File Extension Master";
            return PartialView("FileExtensionAdd");
        }
        [HttpPost]
        public ActionResult Addrecord(FileExtension fileExtensionDetail)

        {
            bool isSuccessStatusCode = false;
            if (ModelState.IsValid)
            {
                var fileExtensionResult = CustomUtility.PostDataOfType("api/SaveFileExtension", fileExtensionDetail, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return RedirectToAction("Index", "FileExtension");

        }
        [HttpPost]
        public JsonResult DeleteFileExtension(int Id)

        {
            bool isSuccessStatusCode = false;
            string delFileExtensionResult = "";
            if (ModelState.IsValid)
            {
                delFileExtensionResult = CustomUtility.PostDataOfType("api/DeleteFileExtension", Id, out isSuccessStatusCode);
            }
            if (!isSuccessStatusCode)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(delFileExtensionResult.ToString());
            }
            return Json("File Extension Deleted Successfully. ");
        }


        public ActionResult EditFileExtension(int Id)
        {
            var FileExtensionResult = CustomUtility.GetSingleRecord<FileExtension>(string.Format("api/GetFileExtension?Id=" + Id));
            return PartialView("FileExtensionEdit", FileExtensionResult[0]);
        }
        [HttpPost]
        public ActionResult EditFileExtension(FileExtension fileExtensionDetail)
        {
            bool isSuccessStatusCode = false;

            var fileExtensionResult = CustomUtility.PostDataOfType("api/UpdateFileExtension", fileExtensionDetail, out isSuccessStatusCode);

            if (!isSuccessStatusCode)
            {
                //  throw new CustomException(responseString);
            }
            return RedirectToAction("Index", "FileExtension");

        }

    }
}