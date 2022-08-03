targetScope = 'subscription'

param location string = deployment().location
param project string
param environment string

@secure()
param apiAccessToken string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${project}-${environment}-${location}'
  location: location
  tags: {
    project: project
    environment: environment
  }
}

module appConfiguration 'appConfiguration.bicep' = {
  scope: resourceGroup
  name: 'appConfiguration'
  params: {
    project: project
    environment: environment
    location: resourceGroup.location
    keyValues: [
      {
        name: 'CacheOptions:Sentinel'
        value: '1'
        contentType: 'Option'
      }
      {
        name: 'CacheOptions:PostDataTimeToLiveInHours'
        value: '24'
        contentType: 'Option'
      }
    ]
  }
}

module appService 'appService.bicep' = {
  scope: resourceGroup
  name: 'appService'
  dependsOn: [
    appConfiguration
  ]
  params: {
    project: project
    environment: environment
    location: resourceGroup.location
    allowedOrigins: [
      'https://dawidjachowicz.com'
      'https://www.dawidjachowicz.com'
    ]
    appSettings: [
      {
        name: 'ApiOptions__AccessToken'
        value: apiAccessToken
      }
      {
        name: 'AppConfigurationOptions__Endpoint'
        value: appConfiguration.outputs.appConfigurationEndpoint
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
      }
    ]
    domains: [
      {
        name: 'dawidjachowicz.com'
        dnsRecordType: 'A'
      }
      {
        name: 'www.dawidjachowicz.com'
        dnsRecordType: 'CName'
      }
    ]
  }
}

module appConfigurationRoleAssignment 'appConfigurationRoleAssignment.bicep' = {
  scope: resourceGroup
  name: 'appConfigurationRoleAssignment'
  params: {
    appConfigurationName: appConfiguration.name
    principalId: appService.outputs.principalId
  }
}

module certificates 'certificate.bicep' = {
  scope: resourceGroup
  name: 'certificates'
  params: {
    location: location
    appServiceName: appService.name
    domains: [
      {
        name: 'dawidjachowicz.com'
        dnsRecordType: 'A'
      }
      {
        name: 'www.dawidjachowicz.com'
        dnsRecordType: 'CName'
      }
    ]
  }
}
