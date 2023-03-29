# Azure AD B2C Demos

## Deploying custom policies

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

### API Connectors

Read more about API Connectors for Azure AD B2C from these links:

- [Use API connectors to customize and extend sign-up user flows and custom policies with external identity data sources](https://learn.microsoft.com/en-us/azure/active-directory-b2c/api-connectors-overview)
- [Add an API connector to a sign-up user flow](https://learn.microsoft.com/en-us/azure/active-directory-b2c/add-api-connector)
- [API connector REST API samples](https://learn.microsoft.com/en-us/azure/active-directory-b2c/api-connector-samples)
  - [user-flow-invitation-code](https://github.com/Azure-Samples/active-directory-b2c-node-sign-up-user-flow-invitation-code)
