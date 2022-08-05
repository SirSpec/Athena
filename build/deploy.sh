az deployment sub create \
    --name athenaDeployment \
    --location easteurope \
    --template-file athena.bicep \
    --parameters project=athena \
    --parameters environment=prod \
    --parameters apiAccessToken=<token>
