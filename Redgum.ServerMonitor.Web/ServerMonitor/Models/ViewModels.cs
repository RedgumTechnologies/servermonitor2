using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Models
{
    public enum Status
    {
        Unknown,
        OK,
        Warning,
        Error
    }

    public class MonitoredServersViewModel
    {
        public List<MonitoredServerViewModel> Servers { get; set; }
        public string RequestUrl { get; internal set; }
    }

    public class MonitoredServerViewModel
    {
        public string Domain { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdatedUtc { get; set; }

        public int PendingUpdateCount { get; set; }
        public int WebsiteCount { get; set; }
        public Status WebsiteStatus { get; set; }
        public Status SqlServerStatus { get; set; }
        public Status SqlDatabaseStatus { get; set; }
        public Status DiskInfoStatus { get; set; }
        public Status DriveInfoStatus { get; set; }
        public Status ServiceStatus { get; set; }

    }
}
