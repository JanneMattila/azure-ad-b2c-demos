# Azure AD B2C Demo

Read these articles before you continue:

[Get started with custom policies in Azure Active Directory B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started)

[Manage Azure AD B2C custom policies with Azure PowerShell](https://docs.microsoft.com/en-us/azure/active-directory-b2c/manage-custom-policies-powershell)

---

Connect to your Azure AD B2C tenant and deploy custom policies:

```powershell
$b2cTenantName = "<yourtenant.onmicrosoft.com>"
$iefAppId = "<guid>"
$proxyAppId = "<guid>"

# Install-Module AzureADPreview
Import-Module AzureADPreview
Connect-AzureAD -TenantId $b2cTenantName

# Deploy custom policies
cd CustomPolicies
.\deploy-all.ps1 `
  -TenantName $b2cTenantName `
  -IdentityExperienceFrameworkAppId $iefAppId `
  -ProxyIdentityExperienceFrameworkAppId $proxyAppId
```
