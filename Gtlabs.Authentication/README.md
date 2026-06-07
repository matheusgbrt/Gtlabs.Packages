# Gtlabs.Authentication

JWT bearer authentication helpers for GT Labs services.

This package configures token validation for microservices. It validates issuer, audience, lifetime, signing key, signing algorithm, required JWT claims, and allowed token types.

## Usage

```csharp
services.Configure<JwtEmissionConfiguration>(configuration.GetSection(JwtEmissionConfiguration.SectionName));

services.AddGtlabsAuthentication(options =>
{
    options.AllowAppAndUserTokens();
});
```

Add authentication and authorization middleware to the HTTP pipeline:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```
