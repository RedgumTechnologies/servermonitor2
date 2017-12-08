using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// loads a file which is an embedded resource
        /// </summary>
        /// <param name="pathFromData">path of the file from the powershell folder (in namespace form, not path form) eg ReadFile("PowerShellScripts.Get-SystemStatus.ps1") </param>
        /// <returns></returns>
        public static string ReadPowershellFile(string pathFromData)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"Redgum.ServerMonitor.Web.ServerMonitor.{pathFromData}");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
