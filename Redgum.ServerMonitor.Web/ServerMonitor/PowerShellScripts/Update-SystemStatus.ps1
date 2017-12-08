# Target URL for Update

$ServerMonitorUrl = "http://status.synergybooking.com/api/v1/"

###### You should not need to edit anything below this ######

function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

# Info variables
$TargetDirectory = Get-ScriptDirectory

# Download the system status script...
$ScriptName = "Get-SystemStatus.ps1"
$ServerUpdatePathUrl = $ServerMonitorUrl + "scripts/GetSystemStatusScript/"
$SourceFile = $ServerUpdatePathUrl
$TargetFile = $TargetDirectory +"\" + $ScriptName
Write-Host ($SourceFile | Format-Table | Out-String)

$client = new-object System.Net.WebClient
$client.DownloadFile($SourceFile,$TargetFile)

# Download the windows update script script...
$ScriptName = "Get-WindowsUpdateStatus.ps1"
$ServerUpdatePathUrl = $ServerMonitorUrl + "scripts/GetWindowsUpdateScript/"
$SourceFile = $ServerUpdatePathUrl
$TargetFile = $TargetDirectory +"\" + $ScriptName
Write-Host ($SourceFile | Format-Table | Out-String)

$client = new-object System.Net.WebClient
$client.DownloadFile($SourceFile,$TargetFile)

# and then download myself for good measure...
$ScriptName = "Update-SystemStatus.ps1"
$ServerUpdatePathUrl = $ServerMonitorUrl + "scripts/GetUpdateScript/"
$SourceFile = $ServerUpdatePathUrl
$TargetFile = $TargetDirectory +"\" + $ScriptName
Write-Host ($SourceFile | Format-Table | Out-String)

$client = new-object System.Net.WebClient
$client.DownloadFile($SourceFile,$TargetFile)

# now, just trigger the windows update script
Invoke-Expression $($TargetDirectory + "\Get-WindowsUpdateStatus.ps1")
