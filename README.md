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
## Compatibility
This library is compatible with the folowing frameworks:

* .NET Framework 4.5;
* .NET Standard 1.1;
* .NET Core 5.0

## Usage
Documentation is available via [wiki](https://github.com/simplesoft-pt/Mediator/wiki) or you can check the [working](https://github.com/simplesoft-pt/Mediator/tree/master/work/) examples or [test](https://github.com/simplesoft-pt/Mediator/tree/master/test) code.

Here is an example of a command handler that also sends some events:
```csharp
public class CreateUserCommand : Command {
  public string Email { get; set; }
  public string Password { get; set; }
}

public class UserCreatedEvent : Event {
  public Guid UserId { get; set; }
}

public class UsersCommandHandler : ICommandHandler<CreateUserCommand> {
  
  private readonly IMediator _mediator;
  
  public UsersCommandHandler(IMediator mediator) {
    _mediator = mediator;
  }
  
  public async Task HandleAsync(CreateUserCommand cmd, CancellationToken ct){
    var userId = Guid.NewGuid();
    
    // try add the user to some store
    
    await _mediator.BroadcastAsync(new UserCreatedEvent {
      UserId = userId
    }, ct);
  }
}
```
