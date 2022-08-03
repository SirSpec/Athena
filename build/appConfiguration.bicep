param deploy bool = true
param location string = resourceGroup().location
param project string
param environment string
param keyValues array

resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = if (deploy) {
  name: 'appcs-${project}-${environment}-${location}'
  location: location
  tags: {
    project: project
    environment: environment
  }
  sku: {
    name: 'free'
  }
}

resource appConfigurationKeyValues 'Microsoft.AppConfiguration/configurationStores/keyValues@2022-05-01' = [for keyValue in keyValues: {
  parent: appConfiguration
  name: keyValue.name
  properties: {
    value: keyValue.value
    contentType: keyValue.contentType
  }
}]

output name string = appConfiguration.name
output appConfigurationEndpoint string = appConfiguration.properties.endpoint
