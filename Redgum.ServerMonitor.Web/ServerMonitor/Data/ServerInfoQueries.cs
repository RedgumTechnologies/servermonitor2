using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
{

    public class ServerInfoQueries
    {
        MonitorDbContext _ctx;
        public ServerInfoQueries(MonitorDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<MonitoredServerViewModel> GetServerInfoSummary()
        {
            var servers = _ctx.Servers.ToList();

            // might as well get all the aggregates into memory
            // more data across the network, but less calls to the db
            var aggregates = _ctx.ServerDataAgregates.ToList();
            var aggregatesByServer = aggregates.GroupBy(a => a.Server.Id).ToDictionary(k => k.Key, v => v.ToList());

            Func<ServerDataModel, string, string> getStat = (server, dataKey) =>
              {
                  if (!aggregatesByServer.ContainsKey(server.Id)) return "";
                  var aggregateForServer = aggregatesByServer[server.Id];
                  var aggregate = aggregateForServer.Where(a => a.DataKey == dataKey).FirstOrDefault();
                  if (aggregate == null) return "";
                  return aggregate.Data;
              };


            return servers.Select(s => new MonitoredServerViewModel()
            {
                Name = s.Name,
                Domain = s.Domain,
                LastUpdatedUtc = s.UpdatedUtc,
                DiskInfoStatus = getStat(s, "DriveInfo").ToStatus(),
                DriveInfoStatus = getStat(s, "DiskInfo").ToStatus(),
                PendingUpdateCount = getStat(s, "PendingUpdateCount").ToNumber(),
                ServiceStatus = getStat(s, "ServiceStatus").ToStatus(),
                SqlDatabaseStatus = getStat(s, "SqlDatabaseInfoStatus").ToStatus(),
                SqlServerStatus = getStat(s, "SqlServerInfoStatus").ToStatus(),
                WebsiteCount = getStat(s, "WebsiteCount").ToNumber(),
                WebsiteStatus = getStat(s, "WebsiteStatus").ToStatus(),
            });
        }

        public ServerModel GetServerDetails(string serverName)
        {
            var settings = _ctx.Settings.ToModel();

            var server = _ctx.Servers.FirstOrDefault(s => s.Name == serverName);
            if (server == null) return null;

            var datas = _ctx.ServerData.Where(s => s.Server.Id == server.Id).ToList();
            var aggregates = _ctx.ServerDataAgregates.Where(s => s.Server.Id == server.Id).ToList();

            var model = new ServerModel()
            {
                Name = server.Name,
                Domain = server.Domain,
                UpdateDateTime = new DateTimeOffset(server.UpdatedUtc),
                SystemSettings = settings,
            };

            model.AddStatsFromDb(datas, aggregates);

            return model;
        }

    }

    static class ServerInfoQueryExtensions
    {

    }


}
