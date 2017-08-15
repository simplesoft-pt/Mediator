# Mediator
Small .NET library that helps with the implementation of mediator pattern.

Using a mediator instance, publish commands and broadcast events to their respective generic handlers.

## Installation
The library is available via [NuGet](https://www.nuget.org/packages?q=SimpleSoft.Mediator) packages:
* [SimpleSoft.Mediator.Abstractions](https://www.nuget.org/packages/SimpleSoft.Mediator.Abstractions/) - interfaces and abstract implementations for commands, events, handlers and mediator;
* [SimpleSoft.Mediator](https://www.nuget.org/packages/SimpleSoft.Mediator/) - implementation of the mediator pattern. Typically is only known by the main project (eg. dependency injection container). It provides command and event logging out of the box, making use of `Microsoft.Extensions.Logging.ILogger<T>` interface;

### Package Manager
```powershell
Install-Package SimpleSoft.Mediator.Abstractions
Install-Package SimpleSoft.Mediator
```

### .NET CLI
```powershell
dotnet add package SimpleSoft.Mediator.Abstractions
dotnet add package SimpleSoft.Mediator
```
