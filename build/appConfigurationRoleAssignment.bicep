param appConfigurationName string
param principalId string

resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2022-05-01' existing = {
  name: appConfigurationName
}

var appConfigurationDataReaderRoleDefinitionId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions',
  '516239f1-63e1-4d78-a4de-a74fb236a071')

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(appConfiguration.id, principalId, appConfigurationDataReaderRoleDefinitionId)
  scope: appConfiguration
  properties: {
    roleDefinitionId: appConfigurationDataReaderRoleDefinitionId
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}
