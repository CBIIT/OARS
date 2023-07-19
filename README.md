# Theradex - NCI Web Reporting

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
    - "ConnectionStrings:DefaultConnection"

### Run Application
```
dotnet run
```
To enable hot reloading: 
```
dotnet watch run
```


## Development

### Blazor Project Structure

This project uses the Blazor Server hosting model: [(Microsoft documentation)](https://docs.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-6.0#blazor-server)

![Blazor arch diagram](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models/_static/blazor-server.png?view=aspnetcore-6.0)


The project structure is as follows:
- Controllers
    - MVC controllers used for authentication routing
- Data
    - Database context, services, and models
    - PowerBI SDK services and models
- Layouts
    - Shared pages layouts
    - Authentication and authorization checks
- Pages
    - Razor pages
- Shared
    - Shared UI components
- wwwroot
    - Public static files

### UI Framework - Blazorise (Bootstrap 5)
This project uses [Blazorise](https://blazorise.com/) as a UI framework. Blazorise is a component library that wraps Bootstrap components for use in Blazor.

Documentation for Blazorise can be found [here](https://blazorise.com/docs/).

### Entity Framework
Entity Framework is used for database access. Details on working with Entity Framework in Blazor can be found in the [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core?view=aspnetcore-6.0).

#### Adding a Model
1. Create a new model class in the `Data/Models` folder
2. Register the model in the `DbContext` class in `Data/WrDbContext.cs`

#### Database Migrations
DB migrations are deployed manually by the Theradex dev team. Details on migration files can be found in [Migrations](./Migrations/README.md).

### Dependency Injection
Shared services are added to the application using .NET Core's built-in dependency injection. Details on working with dependency injection can be found in the [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-6.0).

#### Adding a Service
1. Create a new service class in the `Data/Services` folder
2. Create interfaces for the service in the `Data/Services/Abstract` folder
2. Register the service `Program.cs` by adding the following line to the `ConfigureServices` method:
```csharp
builder.Services.AddScoped<INewService, NewService>();
```

### Authorization
Authorization is handled by the `AuthorizeView` component provided by Blazor. This component checks pre-defined policies based on user claims and displays the appropriate content. Details on working with authorization in Blazor can be found in the [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-6.0#authorization).

Existing authorization checks can be found in `Layouts/AuthLayout.razor`.

Policies are defined in `Program.cs`. The following policies are currently defined:
- `IsAdmin`
- `IsRegistered`


### PowerBI Embedded Integration
See [PowerBI Embedded Integration](./Docs/PowerBI-embedding.md).

### AWS Architecture
See [AWS Architecture](./Docs/AWS-architecture.md). 