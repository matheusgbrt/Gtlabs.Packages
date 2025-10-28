# Gtlabs.DependencyInjections

Conventions for automatic dependency registration. Allows services to be registered as Transient, Scoped, or Singleton via marker interfaces.

## Features

- **Automatic registration**: Classes implementing `ITransientDependency`, `IScopedDependency`, or `ISingletonDependency` are registered automatically.
- **DI extensions**: Methods to register all types or only a specific scope.

## Usage

```csharp
services.RegisterAllDependencies();
```

Or register only one type of dependency:

```csharp
services.RegisterTransientDependencies();
services.RegisterScopedDependencies();
services.RegisterSingletonDependencies();
```

