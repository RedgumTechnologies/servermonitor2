using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
{
    public class ServerInfoSaver
    {

        MonitorDbContext _ctx;
        public ServerInfoSaver(MonitorDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task Update(ServerInfo info,  DateTime nowUtc)
        {
            var settings = _ctx.Settings.ToModel();

            // add or update server info
            var server = _ctx.Servers.Where(s => s.Name == info.Server).FirstOrDefault();

            if (server == null)
            {
                server = new ServerDataModel() { Identifier = Guid.NewGuid() };
                _ctx.Servers.Add(server);
            }

            server.Name = info.Server;
            server.Domain = info.DomainWorkgroup;
            server.UpdatedUtc = nowUtc;

            // add/update/delete server data records
            var dbData = _ctx.ServerData.Where(d => d.Server == server).ToList();
            var dbDataByKey = dbData.ToDictionary(k => k.DataKey, v => v);
            var memoryData = info.ToServerData(settings).ToList();
            memoryData.ForEach(d => d.Server = server);
            var memoryDataByKey = memoryData.ToDictionary(k => k.DataKey, v => v);
            var dataToRemove = dbData.Where(d => !memoryDataByKey.ContainsKey(d.DataKey)).ToList();
            var dataToAdd = memoryData.Where(d => !dbDataByKey.ContainsKey(d.DataKey)).ToList();
            
            // modify 
            dbData.ForEach(d =>
            {
                if (memoryDataByKey.ContainsKey(d.DataKey))
                {
                    var md = memoryDataByKey[d.DataKey];
                    d.Data = md.Data;
                }
            });
            // add 
            dataToAdd.ForEach(d => _ctx.ServerData.Add(d));
            // remove
            dataToRemove.ForEach(d => _ctx.ServerData.Remove(d));

            // add/update/delete server aggregate records
            var dbAggregates = _ctx.ServerDataAgregates.Where(d => d.Server == server).ToList();
            var dbAggregatesByKey = dbAggregates.ToDictionary(k => k.DataKey, v => v);
            var memoryAggregates = info.ToServerAggregateData(settings, nowUtc).ToList();
            memoryAggregates.ForEach(d => d.Server = server);
            var memoryAggregatesByKey = memoryAggregates.ToDictionary(k => k.DataKey, v => v);
            var aggregatesToRemove = dbAggregates.Where(d => !memoryAggregatesByKey.ContainsKey(d.DataKey)).ToList();
            var aggregatesToAdd = memoryAggregates.Where(d => !dbAggregatesByKey.ContainsKey(d.DataKey)).ToList();

            // modify 
            dbAggregates.ForEach(d =>
            {
                if (memoryAggregatesByKey.ContainsKey(d.DataKey))
                {
                    var md = memoryAggregatesByKey[d.DataKey];
                    d.Data = md.Data;
                }
            });
            // add 
            aggregatesToAdd.ForEach(d => _ctx.ServerDataAgregates.Add(d));
            // remove
            aggregatesToRemove.ForEach(d => _ctx.ServerDataAgregates.Remove(d));

         
            // save all changes to db
            await _ctx.SaveChangesAsync();

        }
    }
}
