@description('Name of the Web App')
param webAppName string

@description('Location for all resources')
param location string

@description('App Service Plan ID')
param appServicePlanId string

@description('VNet Subnet ID for integration')
param vnetSubnetId string

@description('Name of the Key Vault')
param keyVaultName string

@description('Application Insights Instrumentation Key')
param appInsightsInstrumentationKey string

@description('Application Insights Connection String')
param appInsightsConnectionString string

@description('SQL Connection String')
@secure()
param sqlConnectionString string

resource webApp 'Microsoft.Web/sites@2022-09-01' = {
  name: webAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|9.0'
      minTlsVersion: '1.2'
      http20Enabled: true
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsightsInstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsConnectionString
        }
        {
          name: 'KeyVault__Name'
          value: keyVaultName
        }
      ]
      connectionStrings: [
        {
          name: 'DefaultConnection'
          connectionString: sqlConnectionString
          type: 'SQLAzure'
        }
      ]
    }
  }
}

resource networkConfig 'Microsoft.Web/sites/networkConfig@2022-09-01' = {
  parent: webApp
  name: 'virtualNetwork'
  properties: {
    subnetResourceId: vnetSubnetId
    swiftSupported: true
  }
}

// Store sensitive information in Key Vault
resource sqlConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  name: '${keyVaultName}/ConnectionStrings--DefaultConnection'
  properties: {
    value: sqlConnectionString
    contentType: 'text/plain'
  }
}

output webAppName string = webApp.name
output webAppHostName string = webApp.properties.defaultHostName
output principalId string = webApp.identity.principalId
