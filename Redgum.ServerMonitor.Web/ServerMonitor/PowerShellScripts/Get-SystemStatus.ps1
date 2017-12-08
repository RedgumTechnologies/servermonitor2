[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') | out-null
[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.Web.Administration') | out-null
# Target URL for Update
#$ServerMonitorUrl = "http://seven/ServerMonitor.Web/api/v1/server/info"
$ServerMonitorUrl = "http://status.synergybooking.com/api/v1/server/info"

###### You should not need to edit anything below this ######
Function Load-Module
{
    param ([parameter(Mandatory = $true)][string] $name)

    $retVal = $true

    if (!(Get-Module -Name $name))
    {
        $retVal = Get-Module -ListAvailable | where { $_.Name -eq $name }

        if ($retVal)
        {
            try
            {
                Import-Module $name -ErrorAction SilentlyContinue
            }

            catch
            {
                $retVal = $false
            }
        }
    }

    return $retVal
}

Function Get-HostUptime {
       param ([string]$ComputerName)
       $Uptime = Get-WmiObject -Class Win32_OperatingSystem -ComputerName $ComputerName
       $LastBootUpTime = $Uptime.ConvertToDateTime($Uptime.LastBootUpTime)
       $Time = (Get-Date) - $LastBootUpTime
	   Return $Time.Ticks
       #Return '{0:00} Days, {1:00} Hours, {2:00} Minutes, {3:00} Seconds' -f $Time.Days, $Time.Hours, $Time.Minutes, $Time.Seconds
}

Function Get-NetWorkInfo {
	$NetWorkInfo = Get-WmiObject win32_networkadapterconfiguration |where {($_.IPEnabled -like "true")} | Select-Object Index,Description,Caption,MACAddress,IPAddress,IPSubnet,DHCPEnabled,DNSDomain,DNSHostName,IPEnabled,DefaultIPGateway
	#$NetWorkInfo = Get-WmiObject win32_networkadapterconfiguration |where {($_.IPEnabled -like "true")}
	#Write-Host ($NetWorkInfo | Format-Table | Out-String)
	Return $NetWorkInfo
}

Function Get-SqlServerStatus{
    param([string]$ComputerName)

    $instInfo = ""
	$objcol = @()
    $SqlInfo = Get-WmiObject win32_service -ComputerName $ComputerName  | where {($_.name -like "MSSQL$*" -or $_.name -like "MSSQLSERVER" -or $_.name -like "SQL Server (*") -and $_.name -notlike "*helper*" -and $_.name -notlike "*Launcher*"}
    if ($SqlInfo)
    {
        $instInfo= $SqlInfo |select Name,StartMode,State, Status				
    }
	#Write-Host ($instInfo | Format-Table | Out-String)
	Return $instInfo
}

Function Get-SqlDatabaseInfo{
    param([string]$ComputerName,$SqlInstanceInfo)

    $sqlobjcol = @()
	foreach ($sqlSqlInstance in $SqlInstanceInfo)
	{
		$instanceName = $ComputerName + "\" + $sqlSqlInstance.Name.Split('$')[1]
	
		#Write-Host($instanceName)
		$s = New-Object ('Microsoft.SqlServer.Management.Smo.Server') $instanceName
		$dbs=$s.Databases

		if ($dbs)
		{
			$sqlDbInfo= $dbs | SELECT Parent, Name, AutoShrink, RecoveryModel, Size, SpaceAvailable, LastBackupDate, IsSystemObject
			if($sqlDbInfo)
			{
				foreach ($sqlDbInstance in $sqlDbInfo)
				{
					$obj = @{
						ComputerName = $ComputerName
						InstanceName = $sqlSqlInstance.Name
						Parent = $sqlDbInstance.Parent
						Name = $sqlDbInstance.Name
						AutoShrink = $sqlDbInstance.AutoShrink
						RecoveryModel = $sqlDbInstance.RecoveryModel
						SizeInMB = $sqlDbInstance.Size
						SpaceAvailableInKB = $sqlDbInstance.SpaceAvailable
						LastBackupDate = $sqlDbInstance.LastBackupDate
						IsSystemObject = $sqlDbInstance.IsSystemObject
					}
					$sqlobjcol += $obj
				}
			}
		}
		
	}

	#Write-Host ($sqlobjcol | Format-Table | Out-String)
    Return $sqlobjcol 
}

Function Post-ToServer{
    param([string]$Url,[string]$data)
    
    # Post the Json data to the server endpoint
    $contentType = "application/json"
    $codePageName = "UTF-8"

    [System.Net.WebRequest]$webRequest = [System.Net.WebRequest]::Create($url);
    $webRequest.ServicePoint.Expect100Continue = $false;
    $webRequest.ContentType = $contentType;
    $webRequest.Method = "POST";

    $enc = [System.Text.Encoding]::GetEncoding($codePageName);
    [byte[]]$bytes = $enc.GetBytes($data);
    $webRequest.ContentLength = $bytes.Length;
    [System.IO.Stream]$reqStream = $webRequest.GetRequestStream();
    $reqStream.Write($bytes, 0, $bytes.Length);
    $reqStream.Flush();

    $resp = $webRequest.GetResponse();
    #Write-Host ($resp | Format-Table | Out-String)
}

# Info variables

$Computer = $env:COMPUTERNAME
$ProccessNumToFetch = 5
$ReportDateTime = Get-Date -DisplayHint DateTime -Format "dd/MM/yyyy hh:mm:ss"

# Information blocks
$SysInfo = Get-WmiObject -Class Win32_ComputerSystem 
$DiskInfo= Get-WMIObject -ComputerName $Computer Win32_LogicalDisk | Where-Object{$_.DriveType -eq 3} | Select-Object VolumeName, Name, @{n='SizeInGB';e={"{0:n2}" -f ($_.size/1gb)}}, @{n='FreeSpaceInGB';e={"{0:n2}" -f ($_.freespace/1gb)}}, @{n='PercentFree';e={"{0:n2}" -f ($_.freespace/$_.size*100)}}
$DriveInfo= Get-WMIObject -ComputerName $Computer Win32_DiskDrive  | Select-Object  DeviceID,Status,StatusInfo,Partitions,Size,CapabilityDescriptions,Model,SerialNumber

$SqlServerInfo = Get-SqlServerStatus $Computer
if($SqlServerInfo) {
	$SqlDatabaseInfo =  Get-SqlDatabaseInfo $Computer @($SqlServerInfo)
}
# Only attempt to pull the Website Data if the WebAdmin module loaded correctly...
if(Load-Module WebAdministration)
{
	$WebsiteInfo = Get-Website | select Name, State, PhysicalPath
	$AppPoolInfo = Get-ChildItem IIS:\apppools
	#Write-Host ($AppPoolInfo | Format-Table | Out-String)
}
$TopCPUProcesses = Get-Process -ComputerName $computer | Sort CPU -Descending | Select ProcessName, @{n='CPUTime';e={'{0:00}:{1:00}:{2:00}' -f ((New-TimeSpan -Seconds $_.CPU).Hours,(New-TimeSpan -Seconds $_.CPU).Minutes,(New-TimeSpan -Seconds $_.CPU).Seconds)}}, @{n='WorkingSetInMB';e={"{0:n2}" -f ($_.WS/1mb)}} -First $ProccessNumToFetch
$TopMemoryProcesses = Get-Process -ComputerName $computer | Sort WS -Descending | Select ProcessName, @{n='CPUTime';e={'{0:00}:{1:00}:{2:00}' -f ((New-TimeSpan -Seconds $_.CPU).Hours,(New-TimeSpan -Seconds $_.CPU).Minutes,(New-TimeSpan -Seconds $_.CPU).Seconds)}}, @{n='WorkingSetInMB';e={"{0:n2}" -f ($_.WS/1mb)}} -First $ProccessNumToFetch

$HostUptimeInTicks = Get-HostUptime $Computer
$Services = Get-WmiObject -Class Win32_Service | Sort-Object displayname
$NetworkInfo = Get-NetWorkInfo
$mem = Get-WmiObject -Class Win32_ComputerSystem 
$os = Get-WmiObject -Class Win32_OperatingSystem  

$DetailedInfo = @{
	HostUptimeInTicks = $HostUptimeInTicks
    TotalPhysicalMemoryInMB = $($mem.TotalPhysicalMemory/1mb)
    LogicalProcessors = $($mem.NumberOfLogicalProcessors)
    Processors = $($mem.NumberOfProcessors)
    Manufacturer = $($mem.Manufacturer)
    Model = $($mem.Model)
    SystemType = $($mem.SystemType)
	WindowsVersionName = $os.Caption 
	WindowsVersion = $os.Version
}
# The @(...) piece converts the contents into an array
$DataSet = @{
            Server = $Computer
            DomainWorkgroup = $SysInfo.Domain
            ReportDateTime = $ReportDateTime
			DetailedInfo = $DetailedInfo
            DiskInfo = @($DiskInfo)
			DriveInfo = @($DriveInfo)
            SqlServerInfo = @($SqlServerInfo)
			SqlDatabaseInfo =  @($SqlDatabaseInfo)
            WebsiteInfo = @($WebsiteInfo)
			AppPoolInfo = @($AppPoolInfo)
            TopMemoryProcesses = @($TopMemoryProcesses)
            TopCpuProcesses = @($TopCPUProcesses)
			Services = @($Services)
			NetworkInfo = @($NetworkInfo)
            }
$Content = $Dataset | ConvertTo-Json
# Write-Output $Content

Post-ToServer $ServerMonitorUrl $Content
