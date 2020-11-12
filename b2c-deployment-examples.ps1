Import-Module AzureADPreview -UseWindowsPowerShell

$b2cTenantName = "jannemattilab2cdemo.onmicrosoft.com"
Connect-AzureAD -TenantId $b2cTenantName

Get-AzureADMSTrustFrameworkPolicy -Id B2C_1_SignUpSignIn -OutputFilePath .\policies\B2C_1_SignUpSignIn.xml
