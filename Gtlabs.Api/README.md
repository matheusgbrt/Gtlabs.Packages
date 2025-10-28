# Gtlabs.Api

Extensions to simplify ASP.NET API configuration, especially Kestrel binding to dynamic addresses and ports using environment variables. Ideal for dynamic deployment scenarios and service discovery integration.

## Usage

Add the package to your project and use the extensions to configure Kestrel with IP/port from the environment:

```csharp
builder.ConfigureKestrelWithNetworkHelper();
```

## Required environment variables
- `SERVICE_ADVERTISE_HOST`: Host/IP to advertise to Consul (used for Fabio integration).
- `SERVICE_ADVERTISE_PORT`: Port to advertise to Consul (used for Fabio integration).