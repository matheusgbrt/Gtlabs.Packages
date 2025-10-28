# Gtlabs.ServiceBus

Abstractions and integrations for messaging based on Rebus/RabbitMQ. Enables automatic handler registration, dynamic routing of commands and messages, and custom serialization for interoperability between services.

## Usage

Add the package to your project and register ServiceBus in the DI pipeline:

```csharp
services.RegisterServiceBus(configuration);
```

## How to implement a new Command

1. Create a class that implements `ICommand`.
2. Add the `[CommandAttribute]` specifying the queue name and the remote corresponding Message type.

```csharp
using Gtlabs.ServiceBus.ServiceBus.Contracts;

[CommandAttribute("my-registered-queue", "Complete.Remote.Message.Namespace")]
public class MyNewCommand : ICommand
{
    public string Property { get; set; }
}
```

## How to implement a new Message

1. Create a class that implements `IMessage`.
2. Add the `[MessageAttribute]` specifying the full type.

```csharp
using Gtlabs.ServiceBus.ServiceBus.Contracts;

[MessageAttribute("Full.Message.Namespace")]
public class MyMessage : IMessage
{
    public int Value { get; set; }
}
```

## Handlers

To process messages, implement a handler using `IHandleMessages<T>`:

```csharp
using Rebus.Handlers;

public class MyNewCommandHandler : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message)
    {
        // Processing logic
    }
}
```

Handlers are automatically registered by the package.

## Required environment variables

- `ServiceBus:ConnectionString`: RabbitMQ connection string (e.g., `amqp://user:pass@host:port/vhost`, usually registered in Consul K/V)
- `AppId`: Unique application identifier (usually registered by Gtlabs.AppRegistration)