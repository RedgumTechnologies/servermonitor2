using Newtonsoft.Json;
using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
{
    public static class ServerInfoExtensions
    {
        public static IEnumerable<ServerDataDataModel> ToServerData(this ServerInfo info, SystemSettings settings)
        {
            return info.MakeServerData(settings).Where(d => d != null);
        }

        static IEnumerable<ServerDataDataModel> MakeServerData(this ServerInfo info, SystemSettings settings)
        {
            Func<string, object, ServerDataDataModel> getModel = (name, value) =>
            {
                if (value == null) return null;

                return new ServerDataDataModel()
                {
                    Identifier = Guid.NewGuid(),
                    DataKey = name,
                    Data = JsonConvert.SerializeObject(value),
                };
            };

            var serviceInfo = info.Services?.Where(s => settings.MonitoredServices.Contains(s.Name)).ToList();

            yield return getModel("DetailedInfo", info.DetailedInfo);
            yield return getModel("DiskInfo", info.DiskInfo);
            yield return getModel("DriveInfo", info.DriveInfo);
            yield return getModel("NetworkInfo", info.NetworkInfo);
            yield return getModel("TopCpuProcesses", info.TopCpuProcesses);
            yield return getModel("TopMemoryProcesses", info.TopMemoryProcesses);
            yield return getModel("WebsiteInfo", info.WebsiteInfo);
            yield return getModel("AppPoolInfo", info.AppPoolInfo);
            yield return getModel("SqlServerInfo", info.SqlServerInfo);
            yield return getModel("SqlDatabaseInfo", info.SqlDatabaseInfo);
            yield return getModel("ServiceInfo", serviceInfo);
            yield return getModel("PendingUpdateInfo", info.PendingUpdates);
        }


        public static IEnumerable<ServerDataAgregateDataModel> ToServerAggregateData(this ServerInfo info, SystemSettings settings, DateTime nowUtc)
        {
            return info.MakeServerAggregateData(settings, nowUtc).Where(s => s != null);
        }

        static IEnumerable<ServerDataAgregateDataModel> MakeServerAggregateData(this ServerInfo info, SystemSettings settings, DateTime nowUtc)
        {
            Func<string, string, ServerDataAgregateDataModel> getModel = (name, value) =>
            {
                if (value == null) return null;

                return new ServerDataAgregateDataModel()
                {
                    Identifier = Guid.NewGuid(),
                    DataKey = name,
                    Data = value,
                };
            };


            if (info.DiskInfo != null) yield return getModel(
                "DiskInfo",
                info.DiskInfo.Any(di => di.PercentFree < 15) ? "Warning" : "OK"
            );

            if (info.DriveInfo != null) yield return getModel(
                "DriveInfo", 
                info.DriveInfo.Any(di => !di.Status.Equals(DriveStatus.OK)) ? "Warning" : "OK"
            );

            if (info.WebsiteInfo != null) yield return getModel(
                "WebsiteCount", 
                info.WebsiteInfo.Count.ToString()
            );

            if (info.WebsiteInfo != null) yield return getModel(
                "WebsiteStatus", 
                info.WebsiteInfo.Where(wsi => wsi != null && String.Equals(wsi.State, "Started", StringComparison.InvariantCultureIgnoreCase)).Count() == info.WebsiteInfo.Where((wsi) => wsi != null).Count() ? "OK" : "Warning"
            );

            if (info.SqlServerInfo != null) yield return getModel(
                "SqlServerInfoStatus", 
                info.SqlServerInfo.Where(ssi => ssi != null && ssi.Status == "OK").Count() == info.SqlServerInfo.Where(ssi => ssi != null).Count() ? "OK" : "Warning"
            );

            // TODO check if this should be utc? it just used "Now" in the original version
            if (info.SqlDatabaseInfo != null) yield return getModel(
                "SqlDatabaseInfoStatus", 
                info.SqlDatabaseInfo.Where(sdi => sdi != null && !sdi.IsSystemObject && (nowUtc - sdi.LastBackupDate).TotalHours < 48).Count() == info.SqlDatabaseInfo.Where(sdi => sdi != null).Count() ? "OK" : "Warning"
            );

            if (info.Services != null) yield return getModel(
                "ServiceStatus",
                info.Services.Any(si => settings.MonitoredServices.Contains(si.Name) && si.State != "Running")
                   ? "Error"
                   : info.Services.Where(si => si != null && si.Status == "OK").Count() == info.Services.Where(si => si != null).Count()
                        ? "OK" : "Warning"
            );

            if (info.PendingUpdates != null) yield return getModel(
                "PendingUpdateCount", 
                info.PendingUpdates.Count.ToString()
            );

        }

    }

}
