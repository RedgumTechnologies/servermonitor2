using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Models
{

    public class SystemSettings
    {
        public List<string> MonitoredServices { get; set; }
    }

    /// <summary>
    /// describes the state of a server
    /// </summary>
    public class ServerInfo
    {
        public string DomainWorkgroup { get; set; }
        public string Server { get; set; }
        public string ReportDateTime { get; set; }

        public DetailedInfo DetailedInfo { get; set; }
        public List<PendingUpdateInfo> PendingUpdates { get; set; }
        public List<SqlServerInfo> SqlServerInfo { get; set; }
        public List<SqlDatabaseInfo> SqlDatabaseInfo { get; set; }
        public List<NetworkInfo> NetworkInfo { get; set; }
        public List<DiskInfo> DiskInfo { get; set; }
        public List<DriveInfo> DriveInfo { get; set; }
        public List<ServiceInfo> Services { get; set; }
        public List<WebsiteInfo> WebsiteInfo { get; set; }
        public List<AppPoolInfo> AppPoolInfo { get; set; }
        public List<ProcessInfo> TopCpuProcesses { get; set; }
        public List<ProcessInfo> TopMemoryProcesses { get; set; }

        ///// <summary>
        ///// Special tracking property for all keyed values
        ///// </summary>
        //public List<RawData> Data { get; set; }
        //public List<RawData> AggregateData { get; set; }
    }

    public class RawData
    {
        public string DataKey { get; set; }
        public string Data { get; set; }
    }

    public class DetailedInfo
    {
        public long HostUptimeInTicks { get; set; }
        public long TotalPhysicalMemoryInMB { get; set; }
        public int LogicalProcessors { get; set; }
        public int Processors { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SystemType { get; set; }
        public string WindowsVersion { get; set; }
        public string WindowsVersionName { get; set; }
        public TimeSpan HostUptime { get { return new TimeSpan(HostUptimeInTicks); } }
    }

    public class SqlServerInfo
    {
        public string Name { get; set; }
        public string StartMode { get; set; }
        public string State { get; set; }
        public string Status { get; set; }

    }
    public class SqlDatabaseInfo
    {
        public string ComputerName { get; set; }
        public string InstanceName { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public bool AutoShrink { get; set; }
        public string RecoveryModel { get; set; }
        public long SizeInMB { get; set; }
        public long SpaceAvailableInKB { get; set; }
        public DateTime LastBackupDate { get; set; }
        public bool IsSystemObject { get; set; }
    }

    public class NetworkInfo
    {
        public string Index { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public string MACAddress { get; set; }
        public string IPAddress { get; set; }
        public string IPSubnet { get; set; }
        public bool DHCPEnabled { get; set; }
        public string DNSDomain { get; set; }
        public string DNSHostName { get; set; }
        public bool IPEnabled { get; set; }
        public string DefaultIPGateway { get; set; }
    }
    public class ServiceInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string StartMode { get; set; }
        public string PathName { get; set; }
        public bool IsWatchedItem { get; set; }

    }
    public class DiskInfo
    {
        public string VolumeName { get; set; }
        public string Name { get; set; }
        public decimal SizeInGB { get; set; }
        public decimal FreeSpaceInGB { get; set; }
        public decimal PercentFree { get; set; }

    }
    public class DriveInfo
    {
        public DriveStatus Status { get; set; }
        public string DeviceID { get; set; }
        public string StatusInfo { get; set; }
        public int Partitions { get; set; }
        public long Size { get; set; }
        public string CapabilityDescriptions { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

    }


    public enum DriveStatus
    {
        Unknown,
        OK,
        Degraded,
        Error,
        PredFail,
        Starting,
        Stopping,
        Service,
        Stressed,
        NonRecover,
        NoContact,
        LostComm
    }

    public class WebsiteInfo
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string PhysicalPath { get; set; }
    }

    public class AppPoolInfo
    {
        public string Name { get; set; }
        public string State { get; set; }
        public bool enable32BitAppOnWin64 { get; set; }
        public string managedRuntimeVersion { get; set; }
        public string managedPipelineMode { get; set; }

    }
    public class ProcessInfo
    {
        public string ProcessName { get; set; }
        public TimeSpan? CPUTime { get; set; }
        public decimal WorkingSetInMB { get; set; }
    }
    public class PendingUpdateInfo
    {
        public string Title { get; set; }
        public int MinDownloadSize { get; set; }
        public int MaxDownloadSize { get; set; }
        public int AutoSelection { get; set; }
        public int AutoDownload { get; set; }
        public bool IsDownloaded { get; set; }
    }

}