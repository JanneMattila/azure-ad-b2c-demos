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
    [string] $ProxyIdentityExperienceFrameworkAppId,
    
    [Parameter(HelpMessage = "Application Insights Instrumentation Key", Mandatory = $true)] 
    [string] $InstrumentationKey,

    [Parameter(HelpMessage = "Logging mode", Mandatory = $true)] 
    [string] $LoggingMode
)

$ErrorActionPreference = "Stop"

$sourceFile = "$PSScriptRoot\$CustomPolicy.xml"
$payload = Get-Content $sourceFile | `
    ForEach-Object { $_ -Replace "yourtenant.onmicrosoft.com", $TenantName } | `
    ForEach-Object { $_ -Replace "~/tenant/templates/AzureBlue/", $ContentRootUri } | `
    ForEach-Object { $_ -Replace "ProxyIdentityExperienceFrameworkAppId", $ProxyIdentityExperienceFrameworkAppId } | `
    ForEach-Object { $_ -Replace "IdentityExperienceFrameworkAppId", $IdentityExperienceFrameworkAppId } | `
    ForEach-Object { $_ -Replace "APPINSIGHTS_INSTRUMENTATIONKEY", $InstrumentationKey } | `
    ForEach-Object { $_ -Replace "APPINSIGHTS_LOGGINGMODE", $LoggingMode } | `
    Out-String

# https://learn.microsoft.com/en-us/graph/api/trustframework-put-trustframeworkpolicy?view=graph-rest-beta
# https://learn.microsoft.com/en-us/powershell/module/microsoft.graph.beta.identity.signins/update-mgbetatrustframeworkpolicy?view=graph-powershell-beta
Invoke-MgGraphRequest `
    -Method Put `
    -Uri "https://graph.microsoft.com/beta/trustFramework/policies/B2C_1A_$($CustomPolicy)/`$value" `
    -ContentType "application/xml" `
    -Body $payload `
    -OutputFilePath temp_$CustomPolicy.xml

# Get-Content "temp_$CustomPolicy.xml"
Remove-Item "temp_$CustomPolicy.xml"
