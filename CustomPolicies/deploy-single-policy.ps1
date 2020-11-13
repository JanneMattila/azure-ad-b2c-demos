Param (
    [Parameter(HelpMessage = "Custom policy name to deploy", Mandatory = $true)] 
    [string] $CustomPolicy,

    [Parameter(HelpMessage = "Tenant name", Mandatory = $true)] 
    [string] $TenantName,

    [Parameter(HelpMessage = "Content definitions root uri", Mandatory = $true)] 
    [string] $ContentRootUri,
    
    [Parameter(HelpMessage = "IEF App Id", Mandatory = $true)] 
    [string] $IdentityExperienceFrameworkAppId,
    
    [Parameter(HelpMessage = "Proxy IEF App Id", Mandatory = $true)] 
    [string] $ProxyIdentityExperienceFrameworkAppId
)

$ErrorActionPreference = "Stop"

$sourceFile = "$PSScriptRoot\$CustomPolicy.xml"
$targetFile = "$PSScriptRoot\Deploy-$CustomPolicy.xml"
Get-Content $sourceFile | `
        ForEach-Object { $_ -Replace "yourtenant.onmicrosoft.com", $TenantName } | `
        ForEach-Object { $_ -Replace "~/tenant/templates/AzureBlue/", $ContentRootUri } | `
        ForEach-Object { $_ -Replace "ProxyIdentityExperienceFrameworkAppId", $ProxyIdentityExperienceFrameworkAppId } | `
        ForEach-Object { $_ -Replace "IdentityExperienceFrameworkAppId", $IdentityExperienceFrameworkAppId } | `
        Set-Content $targetFile

Set-AzureADMSTrustFrameworkPolicy -Id "B2C_1A_$CustomPolicy" -InputFilePath $targetFile | Out-Null
Remove-Item $targetFile
