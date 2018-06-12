using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NPSLWeb.Models;
using Microsoft.AspNetCore.Http;

namespace NPSLWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult StyleSheet(string themeName)
        {
            Response.ContentType = "text/css";
            if (themeName == "Simplex")
            {
                HttpContext.Session.SetString("ThemeSession", "/lib/bootstrap/dist/css/simplex-theme.css");
            }
            else if (themeName == "Cerulean")
            {
                HttpContext.Session.SetString("ThemeSession", "/lib/bootstrap/dist/css/cerulean-theme.css");
            }
            else if (themeName == "Slate")
            {
                HttpContext.Session.SetString("ThemeSession", "/lib/bootstrap/dist/css/slate-theme.css");
            }
            else if (themeName == "Yeti")
            {
                HttpContext.Session.SetString("ThemeSession", "/lib/bootstrap/dist/css/yeti-theme.css");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
