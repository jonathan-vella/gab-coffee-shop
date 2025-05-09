@description('The name of the project')
param projectName string = 'amsterdamcoffee'

@description('The name of the environment')
param environment string = 'prod'

@description('The location for all resources')
param location string = 'swedencentral'

@description('The administrator login for the SQL server')
param sqlAdministratorLogin string

@description('The administrator password for the SQL server')
@secure()
param sqlAdministratorPassword string

@description('The tier of the App Service plan (F1, D1, B1, B2, B3, S1, P1, etc.)')
param appServicePlanTier string = 'B1'

@description('The instance size of the App Service plan (1, 2, 3)')
param appServicePlanInstances int = 1

// Variables for resource naming
var appServicePlanName = '${projectName}-plan-${environment}'
var webAppName = '${projectName}-app-${environment}'
var sqlServerName = '${projectName}-sql-${environment}'
var databaseName = '${projectName}-db-${environment}'
var keyVaultName = '${projectName}vault${environment}'
var appInsightsName = '${projectName}-insights-${environment}'
var vnetName = '${projectName}-vnet-${environment}'
var subnetWebName = 'web-subnet'
var subnetDelegationName = 'Microsoft.Web/serverFarms'

// Virtual Network
module vnet 'modules/vnet.bicep' = {
  name: 'vnet-deployment'
  params: {
    vnetName: vnetName
    location: location
    addressPrefix: '10.0.0.0/16'
    subnetName: subnetWebName
    subnetPrefix: '10.0.0.0/24'
    delegationName: subnetDelegationName
  }
}

// App Service Plan
module appServicePlan 'modules/appServicePlan.bicep' = {
  name: 'appServicePlan-deployment'
  params: {
    appServicePlanName: appServicePlanName
    location: location
    sku: appServicePlanTier
    capacity: appServicePlanInstances
  }
}

// Key Vault
module keyVault 'modules/keyVault.bicep' = {
  name: 'keyVault-deployment'
  params: {
    keyVaultName: keyVaultName
    location: location
    tenantId: subscription().tenantId
  }
}

// Application Insights
module appInsights 'modules/appInsights.bicep' = {
  name: 'appInsights-deployment'
  params: {
    appInsightsName: appInsightsName
    location: location
  }
}

// SQL Server and Database
module sql 'modules/sql.bicep' = {
  name: 'sql-deployment'
  params: {
    sqlServerName: sqlServerName
    location: location
    administratorLogin: sqlAdministratorLogin
    administratorPassword: sqlAdministratorPassword
    databaseName: databaseName
  }
}

// Web App
module webApp 'modules/webApp.bicep' = {
  name: 'webApp-deployment'
  params: {
    webAppName: webAppName
    location: location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    vnetSubnetId: vnet.outputs.subnetId
    keyVaultName: keyVault.outputs.keyVaultName
    appInsightsInstrumentationKey: appInsights.outputs.instrumentationKey
    appInsightsConnectionString: appInsights.outputs.connectionString
    sqlConnectionString: 'Server=tcp:${sql.outputs.sqlServerFqdn},1433;Initial Catalog=${databaseName};Persist Security Info=False;User ID=${sqlAdministratorLogin};Password=${sqlAdministratorPassword};MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
}

// Add access policies for the web app to access Key Vault
module keyVaultAccessPolicy 'modules/keyVaultAccessPolicy.bicep' = {
  name: 'keyVaultAccessPolicy-deployment'
  params: {
    keyVaultName: keyVault.outputs.keyVaultName
    principalId: webApp.outputs.principalId
    tenantId: subscription().tenantId
  }
}

// Outputs
output webAppName string = webApp.outputs.webAppName
output webAppHostName string = webApp.outputs.webAppHostName
output sqlServerFqdn string = sql.outputs.sqlServerFqdn
output keyVaultName string = keyVault.outputs.keyVaultName
