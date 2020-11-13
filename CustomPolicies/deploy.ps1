Param (
    [Parameter(HelpMessage = "Tenant name")] 
    [string] $TenantName = "jannemattilab2cdemo.onmicrosoft.com",

    [Parameter(HelpMessage = "Content definitions root uri")] 
    [string] $ContentRootUri = "https://jannemattilab2cdemo.blob.core.windows.net/b2c/",

    [Parameter(HelpMessage = "IEF App Id")] 
    [string] $IdentityExperienceFrameworkAppId = "7710d04e-f1bf-4ccb-a182-0c24ff1abd9b",
    
    [Parameter(HelpMessage = "Proxy IEF App Id")] 
    [string] $ProxyIdentityExperienceFrameworkAppId = "e527f2ea-baff-4312-abb1-1a190a8ba9b3"
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
        -ProxyIdentityExperienceFrameworkAppId $ProxyIdentityExperienceFrameworkAppId
}
