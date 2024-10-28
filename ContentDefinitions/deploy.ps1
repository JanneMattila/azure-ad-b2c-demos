Param (
    [Parameter(HelpMessage = "Deployment storage account resource group")] 
    [string] $DeploymentResourceGroupName = "rg-b2c",

    [Parameter(HelpMessage = "Deployment storage account name")] 
    [string] $DeploymentStorageName = "jannemattilab2cdemos",

    [Parameter(HelpMessage = "Deployment container")] 
    [string] $DeploymentContainer = "b2c",

    [Parameter(HelpMessage = "Deployment location")] 
    [string] $Location = "Sweden Central",

    [Parameter(HelpMessage = "Tenant url")] 
    [string] $TenantUrl = "jannemattilab2cdemos.b2clogin.com"
)

$ErrorActionPreference = "Stop"

function GetContentType([string] $extension) {
    if ($extension -eq ".cshtml") {
        return "text/html"
    }
    elseif ($extension -eq ".html") {
        return "text/html"
    }
    elseif ($extension -eq ".svg") {
        return "image/svg+xml"
    }
    elseif ($extension -eq ".png") {
        return "image/png"
    }
    elseif ($extension -eq ".css") {
        return "text/css"
    }
    elseif ($extension -eq ".js") {
        return "text/javascript"
    }
    elseif ($extension -eq ".json") {
        return "application/json"
    }
    return "text/plain"
}

# Deployment storage account
if ($null -eq (Get-AzResourceGroup -Name $DeploymentResourceGroupName -Location $Location -ErrorAction SilentlyContinue)) {
    Write-Warning "Resource group '$DeploymentResourceGroupName' doesn't exist and it will be created."
    New-AzResourceGroup -Name $DeploymentResourceGroupName -Location $Location -Verbose
    New-AzStorageAccount `
        -ResourceGroupName $DeploymentResourceGroupName `
        -Name $DeploymentStorageName `
        -Location $Location `
        -SkuName Standard_RAGZRS `
        -EnableHttpsTrafficOnly $true
}

Set-AzCurrentStorageAccount -ResourceGroupName $DeploymentResourceGroupName -Name $DeploymentStorageName

$corsRules = (@{
        AllowedHeaders  = @("*");
        AllowedOrigins  = @("https://$TenantUrl");
        MaxAgeInSeconds = 30;
        AllowedMethods  = @("GET", "OPTIONS")
    })
Set-AzStorageCORSRule -ServiceType Blob -CorsRules $corsRules

New-AzStorageContainer -Name $DeploymentContainer -Permission Blob -ErrorAction SilentlyContinue

$folder = "$PSScriptRoot\"
Get-ChildItem -File -Recurse $folder -Exclude *.ps1 `
| ForEach-Object { 
    $name = $_.FullName.Replace($folder, "")
    $contentType = GetContentType($_.Extension)
    $properties = @{"ContentType" = $contentType }

    $file = Set-AzStorageBlobContent -File $_.FullName -Blob $name -Container $DeploymentContainer -Properties $properties -Force
    Write-Host "Deploying file: $name -> $($file.ICloudBlob.Uri)"
}

$container = Get-AzStorageContainer -Name $DeploymentContainer
"$($container.Context.BlobEndPoint)$($container.Name)/"
