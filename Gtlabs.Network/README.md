# Gtlabs.Network
# INTERNAL USE ONLY
Utilities for network operations, such as preferred local IP discovery, private IP verification, and port selection for services.

## Features

- **Local IP discovery**: Gets the most suitable IP for service binding.
- **Port selection**: Chooses port for Consul/Kestrel, considering configuration and environment.

## Usage

```csharp
NetworkHelper.Initialize(configuration);
var ip = NetworkHelper.GetPreferredLocalIPAddress();
var port = NetworkHelper.GetConsulChosenPort();
```
