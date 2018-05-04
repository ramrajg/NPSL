using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
namespace NPSLWeb.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetUserDetails()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response =  client.GetAsync(path);
            return View();
        }
    }
}
