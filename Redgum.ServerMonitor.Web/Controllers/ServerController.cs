using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redgum.ServerMonitor.Web.Models;
using Redgum.ServerMonitor.Web.ServerMonitor.Data;
using Redgum.ServerMonitor.Web.ServerMonitor.Models;

namespace Redgum.ServerMonitor.Web.Controllers
{
    public class ServerController : Controller
    {
        ServerInfoQueries _serverInfoQueries;
        public ServerController(ServerInfoQueries serverInfoQueries)
        {
            _serverInfoQueries = serverInfoQueries;
        }

        private static string GetUrl(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host.ToString();
//            uriBuilder.Path = request.Path.ToString(); 
            if (request.Host.Port.HasValue) uriBuilder.Port = request.Host.Port.Value;
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri.ToString();
        }

        public ActionResult Index()
        {
            var summaries = _serverInfoQueries.GetServerInfoSummary().ToList();
            var model = new MonitoredServersViewModel()
            {
                Servers = summaries,
                RequestUrl = GetUrl(Request)
            };

            return View(model);
        }


        public ActionResult ViewServer(string serverName)
        {
            var model = _serverInfoQueries.GetServerDetails(serverName);
            return View(model);
        }

    }
}
