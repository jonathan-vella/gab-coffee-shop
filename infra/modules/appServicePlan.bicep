@description('Name of the App Service Plan')
param appServicePlanName string

@description('Location for all resources')
param location string

@description('The SKU of App Service Plan')
param sku string

@description('The capacity of App Service Plan')
param capacity int

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: sku
    capacity: capacity
  }
  kind: 'linux'
  properties: {
    reserved: true // Required for Linux
  }
}

output appServicePlanId string = appServicePlan.id
