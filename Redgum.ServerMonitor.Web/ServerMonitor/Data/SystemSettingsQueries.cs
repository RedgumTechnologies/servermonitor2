using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
{
    public static class SystemSettingsQueries
    {
        public static SystemSettings ToModel(this IQueryable<SettingsDataModel> settings)
        {
            return new SystemSettings()
            {
                MonitoredServices = settings.Where(d => d.DataKey == "MonitoredService").Select(d => d.Data).ToList()
            };
        }

    }
}
