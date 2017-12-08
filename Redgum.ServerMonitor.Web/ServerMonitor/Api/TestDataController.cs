using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redgum.ServerMonitor.Web.ServerMonitor.Data.TestData;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Api
{
    [Route("/api/testdata")]
    public class TestDataController : Controller
    {
        MonitorContextInitialiser _ctxInit;

        public TestDataController(MonitorContextInitialiser ctxInit)
        {
            _ctxInit = ctxInit;
        }

        public IActionResult Index()
        {
            _ctxInit.Initialize();
            return Ok("data created");
        }
    }
}