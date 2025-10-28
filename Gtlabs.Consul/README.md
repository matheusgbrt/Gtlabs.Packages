# Gtlabs.Consul

Full integration with Consul for service registration, HTTP health check, and distributed configuration loading via KV store.

## Features

- **Automatic registration**: Registers the service in Consul at startup, using environment information.
- **Health check**: Exposes a `/health` endpoint for health verification.
- **Distributed configuration**: Loads configuration from Consul's KV store, both global and AppId-specific.
- **Fabio integration**: Supports automatic registration of the service with [Fabio](https://github.com/fabiolb/fabio) via the `RoutePrefix` configuration. The value of `RoutePrefix` is added as a tag in Consul, enabling Fabio to route HTTP traffic to your service.

## Usage
Respect the initialization order:
```csharp
var builder = WebApplication.CreateBuilder(args);
await builder.Configuration.AddConsulConfigurationAsync();
builder.Services.AddConsulRegistration(configuration);
var app = builder.Build();
app.AddConsulHealthCheck();
```

## Required environment variables

- `CONSUL_HOST`: Consul server address.
- `CONSUL_PORT`: Consul server port.
- `APP_ID`: Unique application identifier.
- `SERVICE_ADVERTISE_HOST`: Host/IP to advertise to Consul.
- `SERVICE_ADVERTISE_PORT`: Port to advertise to Consul.
- `RoutePrefix`: HTTP route prefix for Fabio (added as a Consul tag).
