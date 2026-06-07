# Gtlabs.AmbientData

Ambient request and environment data abstractions for GT Labs services.

Provides a single `IAmbientData` service for reading common contextual values such as user id, correlation id, app id, gateway URL, and service token from registered providers.

## Usage

```csharp
services.AddAmbientData();
```

Inject `IAmbientData` where contextual values are needed.
