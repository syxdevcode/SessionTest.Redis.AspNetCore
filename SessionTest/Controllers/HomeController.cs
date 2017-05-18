using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace SessionTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //HttpContext.Session.SetString("key", "strValue");

            //HttpContext.Session.GetString("key");
            var temp = Environment.MachineName;
            ViewData["Label1"] = "服务器IP地址：" + HttpContext.Connection.LocalIpAddress.ToString();//服务器IP地址 
            ViewData["Label2"] = "HTTP访问端口：" + HttpContext.Request.Host.Port;
            ViewData["Label3"] = "虚拟目录Session总数：" + HttpContext.Session.Keys.Count();
            
            if (HttpContext.Session.GetString("test") == null)
                ViewData["Session"] = "";
            else
                ViewData["Session"] = HttpContext.Session.GetString("test");
            return View();
        }

        public IActionResult About()
        {
            HttpContext.Session.SetString("test", Request.Form["Salary"]);

            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
