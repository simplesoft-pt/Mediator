# Mediator
Small .NET library that helps with the implementation of mediator pattern for commands, events and queries.

Using a mediator instance, send commands, broadcast events and fetch queries from their respective generic handlers.

## Installation
The library is available via [NuGet](https://www.nuget.org/packages?q=SimpleSoft.Mediator) packages:

| NuGet | Description | Version |
| --- | --- | --- |
| [SimpleSoft.Mediator.Abstractions](https://www.nuget.org/packages/simplesoft.mediator.abstractions) | interfaces and abstract implementations (commands, events, queries, mediator, ...) | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.abstractions.svg)](https://www.nuget.org/packages/simplesoft.mediator.abstractions) |
| [SimpleSoft.Mediator](https://www.nuget.org/packages/simplesoft.mediator) | library implementation that typically is only known by the main project (eg. dependency injection container) | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.svg)](https://www.nuget.org/packages/simplesoft.mediator) |
| [SimpleSoft.Mediator.Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.dependencyinjection) | specialized methods and classes for the container `Microsoft.Extensions.DependencyInjection` | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.microsoft.extensions.dependencyinjection.svg)](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.dependencyinjection) |
| [SimpleSoft.Mediator.Microsoft.Extensions.Logging](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.logging) | implementation wrappers that support logging using `Microsoft.Extensions.Logging` interfaces | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.mediator.microsoft.extensions.logging.svg)](https://www.nuget.org/packages/simplesoft.mediator.microsoft.extensions.logging) |

### Package Manager
```powershell
Install-Package SimpleSoft.Mediator.Abstractions
Install-Package SimpleSoft.Mediator
Install-Package SimpleSoft.Mediator.Microsoft.Extensions.DependencyInjection
Install-Package SimpleSoft.Mediator.Microsoft.Extensions.Logging
```

### .NET CLI
```powershell
dotnet add package SimpleSoft.Mediator.Abstractions
dotnet add package SimpleSoft.Mediator
dotnet add package SimpleSoft.Mediator.Microsoft.Extensions.DependencyInjection
dotnet add package SimpleSoft.Mediator.Microsoft.Extensions.Logging
```
## Compatibility
This library is compatible with the following frameworks:

* `SimpleSoft.Mediator.Abstractions`
  * .NET Standard 1.0;
* `SimpleSoft.Mediator`
  * .NET Standard 1.0;
* `SimpleSoft.Mediator.Microsoft.Extensions.DependencyInjection`
  * .NET Standard 1.0;
* `SimpleSoft.Mediator.Microsoft.Extensions.Logging`
  * .NET Standard 1.1;

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
