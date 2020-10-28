# Mediator
Small .NET library that helps with the implementation of mediator pattern for commands, events and queries.

Using a mediator instance, send commands, broadcast events and fetch queries from their respective generic handlers.

## Articles

* [Introduction to the mediator pattern in ASP.NET Core applications](https://joaoprsimoes.medium.com/mediator-pattern-in-asp-net-core-applications-109b4231c0f8)

## Installation
The library is available via [NuGet](https://www.nuget.org/packages?q=SimpleSoft.Mediator) packages:

| NuGet | Description | Version |
| --- | --- | --- |
| [SimpleSoft.Mediator.Abstractions](https://www.nuget.org/packages/simplesoft.mediator.abstractions) | interfaces and abstract implementations (commands, events, queries, mediator, ...) | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.abstractions.svg)](https://www.nuget.org/packages/simplesoft.mediator.abstractions) |
| [SimpleSoft.Mediator](https://www.nuget.org/packages/simplesoft.mediator) | core implementation | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.svg)](https://www.nuget.org/packages/simplesoft.mediator) |
| [SimpleSoft.Mediator.Microsoft.Extensions](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions) | specialized methods and classes for the Microsoft dependency injection container and logging facades | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.microsoft.extensions.svg)](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions) |
| [SimpleSoft.Mediator.Microsoft.Extensions.EFCoreTransactionPipeline](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.efcoretransactionpipeline) | mediator pipeline to enforce Entity Framework Core transactions | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.microsoft.extensions.efcoretransactionpipeline.svg)](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.efcoretransactionpipeline) |
| [SimpleSoft.Mediator.Microsoft.Extensions.LoggingPipeline](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.loggingpipeline) | pipeline that serializes commands, queries, events and results into the logging | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.microsoft.extensions.loggingpipeline.svg)](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.loggingpipeline) |
| [SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.validationpipeline) | pipeline that enforces validation into commands, queries and events before entering the handlers by using `FluentValidation` | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.microsoft.extensions.validationpipeline.svg)](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.validationpipeline) |

### Package Manager
```powershell
Install-Package SimpleSoft.Mediator.Abstractions
Install-Package SimpleSoft.Mediator
Install-Package SimpleSoft.Mediator.Microsoft.Extensions
Install-Package SimpleSoft.Mediator.Microsoft.Extensions.EFCoreTransactionPipeline
Install-Package SimpleSoft.Mediator.Microsoft.Extensions.LoggingPipeline
Install-Package SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline
```

### .NET CLI
```powershell
dotnet add package SimpleSoft.Mediator.Abstractions
dotnet add package SimpleSoft.Mediator
dotnet add package SimpleSoft.Mediator.Microsoft.Extensions
dotnet add package SimpleSoft.Mediator.Microsoft.Extensions.EFCoreTransactionPipeline
dotnet add package SimpleSoft.Mediator.Microsoft.Extensions.LoggingPipeline
dotnet add package SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline
```
## Compatibility
This library is compatible with the following frameworks:

* `SimpleSoft.Mediator.Abstractions`
  * .NET Framework 4.0+;
  * .NET Standard 1.0+;
* `SimpleSoft.Mediator`
  * .NET Framework 4.0+;
  * .NET Standard 1.0+;
* `SimpleSoft.Mediator.Microsoft.Extensions`
  * .NET Standard 1.1+;
* `SimpleSoft.Mediator.Microsoft.Extensions.EFCoreTransactionPipeline`
  * .NET Standard 1.3+;
* `SimpleSoft.Mediator.Microsoft.Extensions.LoggingPipeline`
  * .NET Standard 1.1+;
* `SimpleSoft.Mediator.Microsoft.Extensions.ValidationPipeline`
  * .NET Standard 1.1+;

## Usage
Documentation is available via [wiki](https://github.com/simplesoft-pt/Mediator/wiki) or you can check the [working](https://github.com/simplesoft-pt/Mediator/tree/master/work/) examples or [test](https://github.com/simplesoft-pt/Mediator/tree/master/test) code.

Here is an example of a command handler that also sends some events:
```csharp
public class CreateUserCommand : Command {
  public string Email { get; set; }
  public string Password { get; set; }
}

public class UserCreatedEvent : Event {
  public User User { get; set; }
}

public class UserByIdQuery : Query<User> {
  public Guid UserId { get; set; }
}

public class User {
  public Guid Id { get; set; }
  public string Email { get; set; }
}

public class ExampleHandlers : ICommandHandler<CreateUserCommand>, IQueryHandler<UserByIdQuery,User> {
  
  private readonly IMediator _mediator;
  
  public UsersService(IMediator mediator) {
    _mediator = mediator;
  }
  
  public async Task HandleAsync(CreateUserCommand cmd, CancellationToken ct) {
    var userId = Guid.NewGuid();
    
    // try add the user to some store
    
    await _mediator.BroadcastAsync(new UserCreatedEvent {
      User = new User {
        Id = userId,
        Email = cmd.Email
      }
    }, ct);
  }
  
  public async Task<User> HandleAsync(UserByIdQuery query, CancellationToken ct) {
    User user = null;
    
    // search the store by user id
    
    return user;
  }
}
```
