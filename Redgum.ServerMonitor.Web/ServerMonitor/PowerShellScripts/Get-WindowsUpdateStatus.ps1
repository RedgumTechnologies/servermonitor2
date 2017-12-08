[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') | out-null
[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.Web.Administration') | out-null
# Target URL for Update
#$ServerMonitorUrl = "http://seven/Redgum.ServerMonitor.Web/api/v1/server/info"
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

Function Get-Update { 
    $session = New-Object -ComObject Microsoft.Update.Session 
    $searcher = $session.CreateUpdateSearcher() 
    $result = $searcher.Search("IsInstalled=0 and Type='Software'" )
     
    Return $result.Updates | select Title, MinDownloadSize, MaxDownloadSize, AutoSelection, AutoDownload, IsDownloaded
	    
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
    Write-Host ($resp | Format-Table | Out-String)
}
# Info variables

$Computer = $env:COMPUTERNAME
$ProccessNumToFetch = 5
$ReportDateTime = Get-Date -DisplayHint DateTime -Format "dd/MM/yyyy hh:mm:ss"

# Information blocks
$SysInfo = Get-WmiObject -Class Win32_ComputerSystem 


# $PendingUpdates = select Title, MinDownloadSize, MaxDownloadSize, AutoSelection, AutoDownload, IsDownloaded
$PendingUpdates = Get-Update

# The @(...) piece converts the contents into an array
$DataSet = @{
            Server = $Computer
            DomainWorkgroup = $SysInfo.Domain
            ReportDateTime = $ReportDateTime
            PendingUpdates = @($PendingUpdates)
            }
$Content = $Dataset | ConvertTo-Json
Write-Output $Content

Post-ToServer $ServerMonitorUrl $Content
