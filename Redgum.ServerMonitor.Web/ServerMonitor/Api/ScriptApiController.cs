using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Redgum.ServerMonitor.Web.Extensions;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Api
{
    [Route("api/v1/scripts")]
    public class ScriptApiController : Controller
    {
        [HttpGet("GetSystemStatusScript")]
        public IActionResult GetSystemStatusScript()
        {
            return base.Content(Extensions.Extensions.ReadPowershellFile("PowerShellScripts.Get-SystemStatus.ps1"));
        }

        [HttpGet("GetWindowsUpdateScript")]
        public IActionResult GetWindowsUpdateScript()
        {
            return base.Content(Extensions.Extensions.ReadPowershellFile("PowerShellScripts.Get-WindowsUpdateStatus.ps1"));
        }

        [HttpGet("GetUpdateScript")]
        public IActionResult GetUpdateScript()
        {
            return base.Content(Extensions.Extensions.ReadPowershellFile("PowerShellScripts.Update-SystemStatus.ps1"));
        }

        [HttpGet("GetInstallScripts")]
        public IActionResult GetInstallScripts()
        {
            return base.Content(Extensions.Extensions.ReadPowershellFile("PowerShellScripts.Install-ScriptsAndScheduledTask.ps1"));
        }
    }
}