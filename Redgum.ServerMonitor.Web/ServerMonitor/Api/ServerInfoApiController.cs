using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redgum.ServerMonitor.Web.Data;
using Redgum.ServerMonitor.Web.ServerMonitor.Data;
using Redgum.ServerMonitor.Web.ServerMonitor.Models;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Api
{
    [Route("api/v1/server")]
    public class ServerInfoApiController : Controller
    {

        ServerInfoSaver _serverInfoSaver;
        ServerInfoQueries _serverInfoQueries;
        public ServerInfoApiController(ServerInfoSaver serverInfoSaver, ServerInfoQueries serverInfoQueries)
        {
            _serverInfoSaver = serverInfoSaver;
            _serverInfoQueries = serverInfoQueries;
        }


        [HttpPost("Info")]
        public async Task<IActionResult> Info([FromBody]ServerInfo info)
        {
            try
            {
                var nowUtc = DateTime.UtcNow;
                await _serverInfoSaver.Update(info, nowUtc);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //[HttpGet("List")]
        //public IActionResult List()
        //{


        //    IActionResult lResult = default(HttpResponseMessage);
        //    bool suppressErrors = false;
        //    try
        //    {
        //        //Process the Server Info
        //        IRepositoryResult<List<ServerInfo>> result = Repository.ListServerInfo();
        //        if (result.StatusCode == RepositoryStatusCode.OK)
        //        {
        //            // Process the data for the object here

        //            lResult = Request.CreateResponse(HttpStatusCode.OK, result);
        //        }
        //        else
        //        {
        //            //Failed to retrieve the list
        //            lResult = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        if (suppressErrors)
        //        {
        //            lResult = Request.CreateResponse(HttpStatusCode.OK);
        //        }
        //        else
        //        {
        //            lResult = Request.CreateResponse(HttpStatusCode.InternalServerError);
        //        }
        //        this.ProcessException(ex);

        //    }
        //    return lResult;
        //}

    }
}
