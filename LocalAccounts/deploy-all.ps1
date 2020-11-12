Param (
    [Parameter(HelpMessage = "Tenant name", Mandatory = $true)] 
    [string] $TenantName = "jannemattilab2cdemo.onmicrosoft.com"
)

$ErrorActionPreference = "Stop"

$customPolicies = "TrustFrameworkBase", "TrustFrameworkExtensions", "SignUpOrSignin", "ProfileEdit", "PasswordReset"

foreach ($customPolicy in $customPolicies) {
    "Deploying $customPolicy..."
    .\deploy.ps1 -CustomPolicy $customPolicy -TenantName $TenantName
}
