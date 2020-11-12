# Azure AD B2C Demo

[Manage Azure AD B2C custom policies with Azure PowerShell](https://docs.microsoft.com/en-us/azure/active-directory-b2c/manage-custom-policies-powershell)

Connect to your Azure AD B2C tenant:

```powershell
# Install-Module AzureADPreview
Import-Module AzureADPreview

$b2cTenantName = "jannemattilab2cdemo.onmicrosoft.com"
Connect-AzureAD -TenantId $b2cTenantName

Get-AzureADMSTrustFrameworkPolicy -Id B2C_1_SignUpSignIn -OutputFilePath .\policies\B2C_1_SignUpSignIn.xml
```
