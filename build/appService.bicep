param deploy bool = true
param project string
param environment string
param location string = resourceGroup().location
param domains array
param allowedOrigins array
param appSettings array

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = if (deploy) {
  name: 'plan-${project}-${environment}-${location}'
  location: location
  tags: {
    project: project
    environment: environment
  }
  kind: 'linux'
  sku: {
    name: 'B1'
    tier: 'Basic'
    family: 'B'
    size: '1'
    capacity: 1
  }
  properties: {
    reserved: true
  }
}

resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: 'as-${project}-${environment}-${location}'
  location: location
  tags: {
    project: project
    environment: environment
  }
  identity: {
    type: 'SystemAssigned'
  }
  kind: 'app,linux'
  properties: {
    serverFarmId: appServicePlan.id
    clientAffinityEnabled: false
    httpsOnly: true
    siteConfig: {
      alwaysOn: false
      // az webapp list-runtimes
      linuxFxVersion: 'DOTNETCORE|6.0'
      minTlsVersion: '1.2'
      ftpsState: 'FtpsOnly'
      cors: {
        allowedOrigins: allowedOrigins
      }
      appSettings: appSettings
    }
  }
}

@batchSize(1)
resource hostNameBindings 'Microsoft.Web/sites/hostNameBindings@2022-03-01' = [for domain in domains: {
  name: domain.name
  parent: appService
  properties: {
    hostNameType: 'Verified'
    siteName: domain.name
    customHostNameDnsRecordType: domain.dnsRecordType
  }
}]

output principalId string = appService.identity.principalId
