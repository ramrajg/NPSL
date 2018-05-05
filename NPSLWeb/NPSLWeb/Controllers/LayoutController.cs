using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NPSLWeb.Controllers
{
    public class LayoutController : Controller
    {
        public ActionResult LoginUserName()
        {
            return new ContentResult
            {
                Content = HttpContext.Session.GetString("Test")
            };
        }
    }
}