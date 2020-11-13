# Azure AD B2C Demo

Read these articles before you continue:

[Get started with custom policies in Azure Active Directory B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started)

[Manage Azure AD B2C custom policies with Azure PowerShell](https://docs.microsoft.com/en-us/azure/active-directory-b2c/manage-custom-policies-powershell)

---

Connect to your Azure AD B2C tenant, deploy html content and deploy custom policies:

```powershell
$b2cTenantName = "<yourtenant.onmicrosoft.com>"
$b2cUrl = "<yourtenant.b2clogin.com>"
$resourceGroupName = "<your resource group name>"
$storageName = "<your storage account name>"
$iefAppId = "<guid>"
$proxyAppId = "<guid>"
$instrumentationKey = "<app insights instrumentation key>"
$loggingMode = "Development" # or 'Production'

Login-AzAccount
Select-AzSubscription -SubscriptionName "<your subscription name>"

# Deploy html content
.\ContentDefinitions\deploy.ps1 `
  -DeploymentResourceGroupName $resourceGroupName `
  -DeploymentStorageName $storageName `
  -TenantUrl $b2cUrl

# Install-Module AzureADPreview
Import-Module AzureADPreview -UseWindowsPowerShell
Connect-AzureAD -TenantId $b2cTenantName

# Deploy custom policies
.\CustomPolicies\deploy.ps1 `
  -TenantName $b2cTenantName `
  -ContentRootUri $contentRootUri `
  -IdentityExperienceFrameworkAppId $iefAppId `
  -ProxyIdentityExperienceFrameworkAppId $proxyAppId `
  -InstrumentationKey $instrumentationKey `
  -LoggingMode $loggingMode
```
