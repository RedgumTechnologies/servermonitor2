using Newtonsoft.Json;
using Redgum.ServerMonitor.Web.ServerMonitor.Data;
using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor
{
    static class ServerModelProcessorExtensions
    {
        public static T GetFromKey<T>(this IEnumerable<ServerDataAgregateDataModel> aggregateData, string key, Func<string, T> getValueFun)
        {
            var data = aggregateData.FirstOrDefault(a => a.DataKey == key);
            if (data == null) return default(T);
            if (string.IsNullOrWhiteSpace(data.Data)) return default(T);
            return getValueFun(data.Data);
        }

        public static TOut GetFromKey<TIn, TOut>(this IEnumerable<ServerDataDataModel> datas, string key, Func<TIn, TOut> getValueFun)
        {
            var data = datas.FirstOrDefault(a => a.DataKey == key);
            if (data == null) return default(TOut);
            if (string.IsNullOrWhiteSpace(data.Data)) return default(TOut);

            var o = JsonConvert.DeserializeObject<TIn>(data.Data);
            return getValueFun(o);
        }

        public static List<TOut> GetListFromKey<TIn, TOut>(this IEnumerable<ServerDataDataModel> datas, string key, Func<TIn, TOut> getValueFun)
        {
            return datas.GetFromKey<List<TIn>, List<TOut>>(key, o => o.Where(oo=>oo!=null).Select(oo => getValueFun(oo)).ToList())??new List<TOut>();
        }

    }

    static class ServerModelProcessor
    {
        public static void AddStatsFromDb(this ServerModel server, IReadOnlyCollection<ServerDataDataModel> data, IReadOnlyCollection<ServerDataAgregateDataModel> aggregateData)
        {
            if (aggregateData != null)
            {
                server.PendingUpdateCount = aggregateData.GetFromKey("PendingUpdateCount", v => v.ToNumber());
                server.SqlServerStatus = aggregateData.GetFromKey("SqlServerInfoStatus", v => v.ToStatus());
                server.SqlDatabaseStatus = aggregateData.GetFromKey("SqlDatabaseInfoStatus", v => v.ToStatus());
                server.WebsiteStatus = aggregateData.GetFromKey("WebsiteStatus", v => v.ToStatus());
                server.ServiceStatus = aggregateData.GetFromKey("ServiceStatus", v => v.ToStatus());
                server.WebsiteCount = aggregateData.GetFromKey("WebsiteCount", v => v.ToNumber());
                server.DiskInfoStatus = aggregateData.GetFromKey("DiskInfo", v => v.ToStatus());
            }

            if (data != null)
            {
                server.DetailedInfo = data.GetFromKey<DetailedInfo, DetailedInfoModel>("DetailedInfo", o => new DetailedInfoModel(o));
                server.PendingUpdates = data.GetListFromKey<PendingUpdateInfo, PendingUpdateModel>("PendingUpdateInfo", o => new PendingUpdateModel(o));
                server.Disks = data.GetListFromKey<DiskInfo, DiskInfoModel>("DiskInfo", o => new DiskInfoModel(o));
                server.Drives = data.GetListFromKey<DriveInfo, DriveInfoModel>("DriveInfo", o => new DriveInfoModel(o));
                server.Networks = data.GetListFromKey<NetworkInfo, NetworkInfoModel>("NetworkInfo", o => new NetworkInfoModel(o));
                server.CPUProcesses = data.GetListFromKey<ProcessInfo, ProcessModel>("TopCpuProcesses", o => new ProcessModel(o));
                server.MemoryProcesses = data.GetListFromKey<ProcessInfo, ProcessModel>("TopMemoryProcesses", o => new ProcessModel(o));
                server.Websites = data.GetListFromKey<WebsiteInfo, WebsiteInfoModel>("WebsiteInfo", o => new WebsiteInfoModel(o));
                server.AppPools = data.GetListFromKey<AppPoolInfo, AppPoolInfoModel>("AppPoolInfo", o => new AppPoolInfoModel(o));
                server.SqlServers = data.GetListFromKey<SqlServerInfo, SqlServerInfoModel>("SqlServerInfo", o => new SqlServerInfoModel(o));
                server.SqlDatabases = data.GetListFromKey<SqlDatabaseInfo, SqlDatabaseInfoModel>("SqlDatabaseInfo", o => new SqlDatabaseInfoModel(o));
                server.Services = data.GetListFromKey<ServiceInfo, ServiceInfoModel>("ServiceInfo", o => new ServiceInfoModel(o));

            }
        }
    }
}
