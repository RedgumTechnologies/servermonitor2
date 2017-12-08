using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using Redgum.ServerMonitor.Web.Extensions;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Models
{

    public class ServerModel
    {
        public ServerModel()
        {
            DetailedInfo = new DetailedInfoModel();
            Disks = new List<DiskInfoModel>();
            Drives = new List<DriveInfoModel>();
            Networks = new List<NetworkInfoModel>();
            PendingUpdates = new List<PendingUpdateModel>();
            CPUProcesses = new List<ProcessModel>();
            MemoryProcesses = new List<ProcessModel>();
            Websites = new List<WebsiteInfoModel>();
            AppPools = new List<AppPoolInfoModel>();
            SqlServers = new List<SqlServerInfoModel>();
            SqlDatabases = new List<SqlDatabaseInfoModel>();
            Services = new List<ServiceInfoModel>();
            SystemSettings = new SystemSettings();
            Name = String.Empty;
            Domain = String.Empty;
        }

        //public ServerModel(ServerInfo info, SystemSettings settings)
        //    : this()
        //{
        //    Name = info.Server;
        //    Domain = info.DomainWorkgroup;
        //    UpdateDateTime = new DateTimeOffset(DateTime.Parse(info.ReportDateTime));
        //    SystemSettings = settings;
        //    ServerModelProcessor.ProcessServerInfo(this, info);
        //}

        #region Top Level Properties

        [Required]
        [Display(Name = "Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Domain")]
        [StringLength(100)]
        public string Domain { get; set; }

        [Required]
        [Display(Name = "Updated")]
        public DateTimeOffset UpdateDateTime { get; set; }

        #endregion

        public SystemSettings SystemSettings { get; set; }

        #region Aggregate Properties
        public int PendingUpdateCount { get; set; }
        public int WebsiteCount { get; set; }
        public Status WebsiteStatus { get; set; }
        public Status SqlServerStatus { get; set; }
        public Status SqlDatabaseStatus { get; set; }
        public Status DiskInfoStatus { get; set; }
        public Status DriveInfoStatus { get; set; }
        public Status ServiceStatus { get; set; }
        #endregion

        public DetailedInfoModel DetailedInfo { get; set; }
        public List<PendingUpdateModel> PendingUpdates { get; set; }
        public List<DiskInfoModel> Disks { get; set; }
        public List<DriveInfoModel> Drives { get; set; }
        public List<NetworkInfoModel> Networks { get; set; }
        public List<ProcessModel> CPUProcesses { get; set; }
        public List<ProcessModel> MemoryProcesses { get; set; }
        public List<WebsiteInfoModel> Websites { get; set; }
        public List<AppPoolInfoModel> AppPools { get; set; }
        public List<SqlServerInfoModel> SqlServers { get; set; }
        public List<SqlDatabaseInfoModel> SqlDatabases { get; set; }
        public List<ServiceInfoModel> Services { get; set; }
    }

    public class DetailedInfoModel
    {
        public DetailedInfoModel()
        {
        }

        public DetailedInfoModel(DetailedInfo info)
            : this()
        {
            if (info != null)
            {
                Manufacturer = info.Manufacturer;
                Model = info.Model;
                WindowsVersion = info.WindowsVersion;
                WindowsVersionName = info.WindowsVersionName;
                SystemType = info.SystemType;
                Processors = info.Processors;
                LogicalProcessors = info.LogicalProcessors;
                TotalPhysicalMemory = (info.TotalPhysicalMemoryInMB * 1024 * 1024).ToPrettySize(2);
                HostUptime = info.HostUptime;
            }
        }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SystemType { get; set; }
        public int Processors { get; set; }
        public int LogicalProcessors { get; set; }
        public string TotalPhysicalMemory { get; set; }

        public string WindowsVersionName { get; set; }
        public string WindowsVersion { get; set; }
        public TimeSpan HostUptime { get; set; }


    }

    public class PendingUpdateModel
    {
        public PendingUpdateModel()
        {

        }

        public PendingUpdateModel(PendingUpdateInfo info)
            : this()
        {
            Title = info.Title;
            IsDownloaded = info.IsDownloaded;
            AutoDownload = (info.AutoDownload == 0) ? false : true;
            MinDownloadSize = info.MinDownloadSize;
            MaxDownloadSize = info.MaxDownloadSize;
            if (MinDownloadSize == MaxDownloadSize)
            {
                DownloadSize = MaxDownloadSize.ToPrettySize(2);
            }
            else
            {
                DownloadSize = MinDownloadSize.ToPrettySize(2) + '-' + MaxDownloadSize.ToPrettySize(2);
            }
        }

        public string Title { get; set; }
        public bool IsDownloaded { get; set; }
        public bool AutoDownload { get; set; }
        public int MinDownloadSize { get; set; }
        public int MaxDownloadSize { get; set; }
        public string DownloadSize { get; set; }
    }

    public class DiskInfoModel
    {
        public DiskInfoModel() { }

        public DiskInfoModel(DiskInfo info)
            : this()
        {
            VolumeName = info.VolumeName;
            Name = info.Name;
            SizeInGB = info.SizeInGB;
            FreeSpaceInGB = info.FreeSpaceInGB;
            PercentFree = info.PercentFree;
        }

        public string VolumeName { get; set; }
        public string Name { get; set; }
        public decimal SizeInGB { get; set; }
        public decimal FreeSpaceInGB { get; set; }
        public decimal PercentFree { get; set; }
    }

    public class DriveInfoModel
    {
        public DriveInfoModel() { }
        public DriveInfoModel(DriveInfo info)
            : this()
        {
            DeviceID = info.DeviceID;
            Status = info.Status;
            StatusInfo = info.StatusInfo;
            Partitions = info.Partitions;
            Size = info.Size.ToPrettySize(2);
            CapabilityDescriptions = info.CapabilityDescriptions;
            Model = info.Model;
            SerialNumber = info.SerialNumber;
        }
        public string DeviceID { get; set; }
        public DriveStatus  Status { get; set; }
        public string StatusInfo { get; set; }
        public int Partitions { get; set; }
        public string Size { get; set; }
       
        public string CapabilityDescriptions { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

    }

    public class NetworkInfoModel
    {
        public NetworkInfoModel() { }
        public NetworkInfoModel(NetworkInfo info)
            : this()
        {
            Index = info.Index;
            Description = info.Description;
            Caption = info.Caption;
            MACAddress = info.MACAddress;
            IPAddress = info.IPAddress;
            IPSubnet = info.IPSubnet;
            DHCPEnabled = info.DHCPEnabled;
            DNSDomain = info.DNSDomain;
            DNSHostName = info.DNSHostName;
            IPEnabled = info.IPEnabled;
            DefaultIPGateway = info.DefaultIPGateway;

        }
        public string Index { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public string MACAddress { get; set; }
        public string IPAddress { get; set; }
        public string IPSubnet { get; set; }
        public Boolean DHCPEnabled { get; set; }
        public string DNSDomain { get; set; }
        public string DNSHostName { get; set; }
        public Boolean IPEnabled { get; set; }
        public string DefaultIPGateway { get; set; }

    }

    public class ProcessModel
    {
        public ProcessModel() { }

        public ProcessModel(ProcessInfo info)
            : this()
        {
            ProcessName = info.ProcessName;
            CPUTime = info.CPUTime;
            WorkingSetInMB = info.WorkingSetInMB;
        }

        public string ProcessName { get; set; }
        public TimeSpan? CPUTime { get; set; }
        public decimal WorkingSetInMB { get; set; }
    }

    public class SqlServerInfoModel
    {
        public SqlServerInfoModel() { }
        public SqlServerInfoModel(SqlServerInfo info)
            : this()
        {
            Name = info.Name;
            StartMode = info.StartMode;
            State = info.State;
            Status = info.Status;

        }
        public string Name { get; set; }
        public string StartMode { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
    }

    public class SqlDatabaseInfoModel
    {
        public SqlDatabaseInfoModel() { }
        public SqlDatabaseInfoModel(SqlDatabaseInfo info)
            : this()
        {
            Name = info.Name;
            InstanceName = info.InstanceName;
            IsSystemObject = info.IsSystemObject;
            if (!info.LastBackupDate.Equals(new DateTime()))
            {
                LastBackupDate = info.LastBackupDate;
            }

            SizeInBytes = info.SizeInMB * 1024 * 1024;
            Size = SizeInBytes.ToPrettySize(2);
            SpaceAvailableInBytes = info.SpaceAvailableInKB * 1024;
            SpaceAvailable = SpaceAvailableInBytes.ToPrettySize(2);
        }

        public String Name { get; set; }
        public String InstanceName { get; set; }
        public Boolean IsSystemObject { get; set; }
        public DateTime? LastBackupDate { get; set; }
        public long SizeInBytes { get; set; }
        public string Size { get; set; }
        public long SpaceAvailableInBytes { get; set; }
        public string SpaceAvailable { get; set; }
    }

    public class WebsiteInfoModel
    {
        public WebsiteInfoModel() { }
        public WebsiteInfoModel(WebsiteInfo info)
            : this()
        {
            Name = info.Name;
            State = info.State;
            PhysicalPath = info.PhysicalPath;
        }
        public string Name { get; set; }
        public string PhysicalPath { get; set; }
        public string State { get; set; }
    }

    public class AppPoolInfoModel
    {
        public AppPoolInfoModel() { }
        public AppPoolInfoModel(AppPoolInfo info)
            : this()
        {
            Name = info.Name;
            State = info.State;
            enable32BitAppOnWin64 = info.enable32BitAppOnWin64;
            managedPipelineMode = info.managedPipelineMode;
            managedRuntimeVersion = info.managedRuntimeVersion;

        }
        public string Name { get; set; }
        public string State { get; set; }
        public string managedPipelineMode { get; set; }
        public string managedRuntimeVersion { get; set; }
        public Boolean enable32BitAppOnWin64 { get; set; }

    }
    public class ServiceInfoModel
    {
        public ServiceInfoModel() { }
        public ServiceInfoModel(ServiceInfo info)
            : this()
        {
            Name = info.Name;
            DisplayName = info.DisplayName;
            Description = info.Description;
            Status = info.Status;
            State = info.State;
            StartMode = info.StartMode;
            IsWatchedItem = info.IsWatchedItem;
        }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string StartMode { get; set; }
        public bool IsWatchedItem { get; set; }

    }



}