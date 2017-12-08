#####################################################################################
#
#	Install for Server Monitor
#
#
#	based on the concept of the Chocolaty install
#
#####################################################################################
#
# TODO: get the $TargetDirectory install directory from the command line or an enviroment var
# TODO: get the service install section working again
#
#####################################################################################

# Target URL for Update
$ServerMonitorUrl = "http://status.synergybooking.com/api/v1/"
$TargetDirectory = "C:\APS\_Scripts"

###### You should not need to edit anything below this ######
New-Item -ItemType Directory -Force -Path $TargetDirectory


# Download the update script...
$ScriptName = "Get-SystemStatus.ps1"
$ServerUpdatePathUrl = $ServerMonitorUrl + "scripts/GetSystemStatusScript/"
$SourceFile = $ServerUpdatePathUrl
$TargetFile = $TargetDirectory +"\" + $ScriptName
#Write-Host ($SourceFile | Format-Table | Out-String)

$client = new-object System.Net.WebClient
$client.DownloadFile($SourceFile,$TargetFile)

#Fire the update script for good measure
#Invoke-Expression $($TargetDirectory + "\" + $ScriptName)

# Download the windows update script script...
$ScriptName = "Get-WindowsUpdateStatus.ps1"
$ServerUpdatePathUrl = $ServerMonitorUrl + "scripts/GetWindowsUpdateScript/"
$SourceFile = $ServerUpdatePathUrl
$TargetFile = $TargetDirectory +"\" + $ScriptName
Write-Host ($SourceFile | Format-Table | Out-String)

$client = new-object System.Net.WebClient
$client.DownloadFile($SourceFile,$TargetFile)

# and then download the update script...
$ScriptName = "Update-SystemStatus.ps1"

$ServerUpdatePathUrl = $ServerMonitorUrl + "scripts/GetUpdateScript/"
$SourceFile = $ServerUpdatePathUrl
$TargetFile = $TargetDirectory +"\" + $ScriptName
#Write-Host ($SourceFile | Format-Table | Out-String)

$client = new-object System.Net.WebClient
$client.DownloadFile($SourceFile,$TargetFile)

# Now set up the scheduled task for the Status Reporting
#
#$Computer = $env:COMPUTERNAME
#$taskAuthor = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
#$principal = New-ScheduledTaskPrincipal -UserId "LOCALSERVICE" -LogonType ServiceAccount -RunLevel Highest
#
#
#$service = new-object -com("Schedule.Service")
#$service.Connect($Hostname)
#$rootFolder = $service.GetFolder("\")
#
##Now Build the scheduled task for the Update System Status script
#$command = 'powershell.exe'
#$CommandArguments = $TargetDirectory + '\Get-SystemStatus.ps1'
#
#$taskDefinition = $service.NewTask(0)
#
#Write-Host ($SourceFile | Format-Table | Out-String)
#$taskDefinition.Principal = $principal
#$regInfo = $taskDefinition.RegistrationInfo
#$regInfo.Description = 'APS System Status Reporting'
#$regInfo.Author = $taskAuthor
#$settings = $taskDefinition.Settings
#$settings.Enabled = $True
#$settings.StartWhenAvailable = $True
#$settings.Hidden = $False
#$settings.WakeToRun = $True
#$triggers = $taskDefinition.Triggers
#$trigger = $triggers.Create(2)
#$trigger.StartBoundary = "2014-01-01T01:00:00"
#$trigger.ExecutionTimeLimit = "PT5M" # time out in 5 minutes 
#$trigger.DaysInterval = 1
##$Trigger.Userid = $taskAuthor # must be a valid user 
#$trigger.Id = "DailyTriggerId"
#$trigger.Enabled = $True
#$repetition = $trigger.Repetition
#$repetition.Interval = "PT10M"	# every 10 minutes
#$repetition.Duration  = "P1D"
#$repetition.StopAtDurationEnd = $True
#
#$Action = $taskDefinition.Actions.Create(0)
#$Action.Path = $command
#$Action.Arguments = $CommandArguments
#
#$Rootfolder.RegisterTaskDefinition('APS System Status Reporting', $TaskDefinition, 6, "System", $null , 5)
#
##Now Build the scheduled task for the AutoUpdate Script
#$command = 'powershell.exe'
#$CommandArguments = $TargetDirectory + '\Update-SystemStatus.ps1'
#
#$taskDefinition = $service.NewTask(0)
#$taskDefinition.Principal = $principal
#$regInfo = $taskDefinition.RegistrationInfo
#$regInfo.Description = 'Auto-Updates the powershell scripts for system status reporting'
#$regInfo.Author = $taskAuthor
#$settings = $taskDefinition.Settings
#$settings.Enabled = $True
#$settings.StartWhenAvailable = $True
#$settings.Hidden = $False
#$settings.WakeToRun = $True
#$triggers = $taskDefinition.Triggers
#$trigger = $triggers.Create(2)
#$trigger.StartBoundary = "2014-01-01T00:45:00"
#$trigger.ExecutionTimeLimit = "PT5M" # time out in 5 minutes 
#$trigger.DaysInterval = 1
##$Trigger.Userid = $taskAuthor # must be a valid user 
#$trigger.Id = "DailyTriggerId"
#$trigger.Enabled = $True
#
#$Action = $taskDefinition.Actions.Create(0)
#$Action.Path = $command
#$Action.Arguments = $CommandArguments
#
#$Rootfolder.RegisterTaskDefinition('APS System Status Auto-Update', $TaskDefinition, 6, "System", $null , 5)
#
