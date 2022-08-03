param deploy bool = true
param location string = resourceGroup().location
param appServiceName string
param domains array

resource appService 'Microsoft.Web/sites@2022-03-01' existing = {
  name: appServiceName
}

resource certificates 'Microsoft.Web/certificates@2022-03-01' = [for domain in domains: if (deploy) {
  name: domain
  location: location
  dependsOn: [
    appService
  ]
  properties: {
    canonicalName: domain
    serverFarmId: appService.properties.serverFarmId
    domainValidationMethod: 'http-token'
  }
}]

@batchSize(1)
resource hostNameBindings 'Microsoft.Web/sites/hostNameBindings@2022-03-01' = [for (domain, index) in domains: {
  name: domain.name
  parent: appService
  dependsOn: [
    certificates
  ]
  properties: {
    hostNameType: 'Verified'
    siteName: domain.name
    customHostNameDnsRecordType: domain.dnsRecordType
    thumbprint: certificates[index].properties.thumbprint
    sslState: 'SniEnabled'
    azureResourceType: 'Website'
  }
}]
