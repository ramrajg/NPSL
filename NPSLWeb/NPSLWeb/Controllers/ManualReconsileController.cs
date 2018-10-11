using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using NPSLWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace NPSLWeb.Controllers
{
    public class ManualReconsileController : Controller
    {
        public IActionResult Index()
        {
           var formattedDate = DateTime.Today.ToString("dd MMM yyyy");
            var ReconsileReportResult = CustomUtility.GetSingleRecord<NonReconsileData>(string.Format("api/GetNonReconsileData?groupTemplateId=0&fromDate=" + formattedDate + "&toDate=" + formattedDate + ""));
            var TemplateGroupResult = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue?OnlyActive=1"));
            ViewBag.TemplateGroupList = new SelectList(TemplateGroupResult, "templateGroupId", "templateGroupName").Items;
            ViewModelNonReconsile mymodel = new ViewModelNonReconsile();
            mymodel.NonReconsileData = ReconsileReportResult;
            mymodel.TemplateGroup = TemplateGroupResult;
            return View(mymodel);
        }

        [HttpPost]
        public PartialViewResult RefreshSearchResult(int groupId,DateTime FromDate,DateTime ToDate)
        {
            var From_Date = FromDate.ToString("MMM dd yyyy");
            var To_Date = ToDate.ToString("MMM dd yyyy");
            var ReconsileReportResult = CustomUtility.GetSingleRecord<NonReconsileData>(string.Format("api/GetNonReconsileData?groupTemplateId=" + groupId + "&fromDate=" + From_Date + "&toDate=" + To_Date + ""));
            ViewModelNonReconsile mymodel = new ViewModelNonReconsile();
            mymodel.NonReconsileData = ReconsileReportResult;
            return PartialView("_ManualReconsile", mymodel);
        }

        [HttpPost]
        public PartialViewResult ManualReconsile(List<ManualResult> selectedResult,int groupId, DateTime FromDate, DateTime ToDate)
        {
            bool isSuccessStatusCode = false;
            var From_Date = FromDate.ToString("MMM dd yyyy");
            var To_Date = ToDate.ToString("MMM dd yyyy");
            var roleInfoResult = CustomUtility.PostDataOfType("api/ProcessManualReconsile", selectedResult, out isSuccessStatusCode);
            var ReconsileReportResult = CustomUtility.GetSingleRecord<NonReconsileData>(string.Format("api/GetNonReconsileData?groupTemplateId=" + groupId + "&fromDate=" + From_Date + "&toDate=" + To_Date + ""));
            var TemplateGroupResult = CustomUtility.GetSingleRecord<TemplateGroup>(string.Format("api/GetTemplateGroupValue?OnlyActive=1"));
            ViewModelNonReconsile mymodel = new ViewModelNonReconsile();
            mymodel.NonReconsileData = ReconsileReportResult;
            mymodel.TemplateGroup = TemplateGroupResult;
            return PartialView("_ManualReconsile", mymodel);
        }

    }
}