# Theradex Portal

## Local Enviroment Setup

### Requirements
- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Visual Studio 2022 or Visual Studio Code

### Initial Setup
- Add .NET Core Development certificates
```
dotnet --info
dotnet dev-certs https --trust
```
- Add secret configuration with [.NET Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#secret-manager)
```
dotnet user-secrets set "Secret:Name" "12345"
```
- The following secrets are required (current values can be found in AWS Secrets Manager)
    - "PowerBICredentials:TenantId"
    - "PowerBICredentials:ClientSecret"
    - "PowerBICredentials:ClientId"
    - "CognitoConfig:ClientSecret"
    - "CognitoConfig:ClientId"
    - "CognitoConfig:Authority"

### Run Application
```
dotnet run
```
To enable hot reloading: 
```
dotnet watch run
```