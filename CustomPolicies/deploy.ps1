Param (
    [Parameter(HelpMessage = "Tenant name")] 
    [string] $TenantName = "jannemattilab2cdemos.onmicrosoft.com",

    [Parameter(HelpMessage = "Content definitions root uri")] 
    [string] $ContentRootUri = "https://jannemattilab2cdemos.blob.core.windows.net/b2c/",

    [Parameter(HelpMessage = "IEF App Id")] 
    [string] $IdentityExperienceFrameworkAppId = "fa0dd112-68a3-4d62-8ab7-c0d24d0fdf63",
    
    [Parameter(HelpMessage = "Proxy IEF App Id")] 
    [string] $ProxyIdentityExperienceFrameworkAppId = "695069f2-f525-4d91-94cd-604eadf458a5",
    
    [Parameter(HelpMessage = "Application Insights Instrumentation Key")] 
    [string] $InstrumentationKey = "21baea28-c787-4eba-b401-2f282003fa1e",

    [Parameter(HelpMessage = "Logging mode")] 
    [string] $LoggingMode = "Development"
)

$ErrorActionPreference = "Stop"

$customPolicies = "TrustFrameworkBase", "TrustFrameworkExtensions", "SignUpOrSignin", "ProfileEdit", "PasswordReset"

foreach ($customPolicy in $customPolicies) {
    "Deploying $customPolicy..."
    . "$PSScriptRoot\deploy-single-policy.ps1" `
        -CustomPolicy $customPolicy `
        -TenantName $TenantName `
        -ContentRootUri $ContentRootUri `
        -IdentityExperienceFrameworkAppId $IdentityExperienceFrameworkAppId `
        -ProxyIdentityExperienceFrameworkAppId $ProxyIdentityExperienceFrameworkAppId `
        -InstrumentationKey $InstrumentationKey `
        -LoggingMode $LoggingMode
}
