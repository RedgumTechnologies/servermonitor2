using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redgum.ServerMonitor.Web.Models;
using Redgum.ServerMonitor.Web.Data;
using Redgum.ServerMonitor.Web.ServerMonitor.Data;

namespace Redgum.ServerMonitor.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Server");
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
    }
}
