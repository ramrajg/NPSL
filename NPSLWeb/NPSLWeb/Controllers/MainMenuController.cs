using Microsoft.AspNetCore.Mvc;
using NPSLCore.Models.DB;
using NPSLWeb.Helper;
using ServiceBrokerListener.Domain;
using System.Collections.Generic;

namespace NPSLWeb.Controllers
{
    public class MainMenuController : Controller
    {
        public List<Dashboard> DashBoardRefreshList;
        // GET: /<controller>/
        public IActionResult Index()
        {
            var dashBoardInfoResult = CustomUtility.GetSingleRecord<Dashboard>(string.Format("api/GetDashboardData"));
            return View(dashBoardInfoResult);
        }
        [HttpGet]
        public PartialViewResult RefreshData()
        {
            if (DashBoardRefreshList == null)
            {
                RefreshCacheList();
            }
            return PartialView("_DashBoard", DashBoardRefreshList);
        }
        public void RefreshCacheList()
        {
            DashBoardRefreshList = CustomUtility.GetSingleRecord<Dashboard>(string.Format("api/GetDashboardData"));
        }
    }
}
