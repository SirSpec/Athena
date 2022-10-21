# Athena

[![Build Status](https://dev.azure.com/djacho11/Athena/_apis/build/status/Build%20and%20Test?branchName=master)](https://dev.azure.com/djacho11/Athena/_build/latest?definitionId=5&branchName=master)

Blog posts have to created according to the convention:
- Unique file name and is stored inside `posts` directory
- A file name has to match the following regex pattern `^[A-Za-z][A-Za-z\d-]*[A-Za-z\d]$`
- Use the following tokens:
    - Title
    - PublishingDate
    - Description
    - Body
- Posts can contain HTML tags

### Example blog post:
Name: `example-post`
```
{Title}
Example
{Title}
{PublishingDate}
01.01.2022
{PublishingDate}
{Description}
Example Description.
{Description}
{Body}
Example body.
{Body}
```

# Development environment
A developer can define a custom `appsettings.User.json` which overrides settings of:
- `appsettings.{Environment}.json`
- environmental variables

`appsettings.User.json` is not serialized in GitHub repository

In order to access APIs a developer has to define the following settings:
- GitHub API: `ApiOptions:AccessToken`
- Azure App Configuration Service: `AppConfigurationOptions:Endpoint`

# Tech stack
- .NET 6
- ASP.NET Core
- Azure Bicep
- GitHub API

# License
[MIT](LICENSE) © [Dawid Jachowicz](https://github.com/SirSpec)