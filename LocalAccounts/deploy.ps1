Param (
    [Parameter(HelpMessage = "Custom policy name to deploy", Mandatory = $true)] 
    [string] $CustomPolicy,

    [Parameter(HelpMessage = "Tenant name", Mandatory = $true)] 
    [string] $TenantName = "jannemattilab2cdemo.onmicrosoft.com"
)

$ErrorActionPreference = "Stop"

$sourceFile = "$PSScriptRoot\$CustomPolicy.xml"
$targetFile = "$PSScriptRoot\Deploy-$CustomPolicy.xml"
Get-Content $sourceFile | `
        ForEach-Object { $_ -Replace "yourtenant.onmicrosoft.com", $TenantName } | `
        Set-Content $targetFile

Set-AzureADMSTrustFrameworkPolicy -Id "B2C_1A_$CustomPolicy" -InputFilePath $targetFile | Out-Null
Remove-Item $targetFile
