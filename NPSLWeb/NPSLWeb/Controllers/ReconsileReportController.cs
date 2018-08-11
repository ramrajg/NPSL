using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NPSLWeb.Controllers
{
    public class ReconsileReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}