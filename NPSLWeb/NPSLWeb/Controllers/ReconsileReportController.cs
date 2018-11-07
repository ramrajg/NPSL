using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using NPSLWeb.Models;

namespace NPSLWeb.Controllers
{
    public class ReconsileReportController : Controller
    {
        public IActionResult Index()
        {
           var formattedDate = DateTime.Today.ToString("dd MMM yyyy");
            var ReconsileReportResult = CustomUtility.GetSingleRecord<ReconsileReportData>(string.Format("api/GetReconsileReportData?groupTemplateId=0&reconsileType=0&fromDate=" + formattedDate + "&toDate=" + formattedDate + ""));
            var TemplateGroupResult = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue?OnlyActive=1"));
            ViewBag.TemplateGroupList = new SelectList(TemplateGroupResult, "templateGroupId", "templateGroupName").Items;
            ViewModel mymodel = new ViewModel();
            mymodel.ReportData = ReconsileReportResult;
            mymodel.TemplateGroup = TemplateGroupResult;
            return View(mymodel);
        }

        [HttpPost]
        public PartialViewResult RefreshSearchResult(int groupId, int reconsileType, DateTime FromDate,DateTime ToDate)
        {
            var From_Date = FromDate.ToString("MMM dd yyyy");
            var To_Date = ToDate.ToString("MMM dd yyyy");
            var ReconsileReportResult = CustomUtility.GetSingleRecord<ReconsileReportData>(string.Format("api/GetReconsileReportData?groupTemplateId=" + groupId + "&reconsileType=" + reconsileType + "&fromDate=" + From_Date + "&toDate=" + To_Date + ""));
            ViewModel mymodel = new ViewModel();
            mymodel.ReportData = ReconsileReportResult;
            return PartialView("_ReconsileReport", mymodel);
        }

    }
}