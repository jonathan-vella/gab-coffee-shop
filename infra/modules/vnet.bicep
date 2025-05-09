@description('Name of the Virtual Network')
param vnetName string

@description('Location for all resources')
param location string

@description('Address prefix for the Virtual Network')
param addressPrefix string

@description('Name of the subnet')
param subnetName string

@description('Subnet address prefix')
param subnetPrefix string

@description('Name of the delegation service')
param delegationName string

resource vnet 'Microsoft.Network/virtualNetworks@2022-05-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressPrefix
      ]
    }
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: subnetPrefix
          delegations: [
            {
              name: '${subnetName}-delegation'
              properties: {
                serviceName: delegationName
              }
            }
          ]
        }
      }
    ]
  }
}

output vnetId string = vnet.id
output subnetId string = vnet.properties.subnets[0].id
