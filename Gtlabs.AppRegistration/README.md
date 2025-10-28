# Gtlabs.AppRegistration

Facilita o registro da aplicação, configurando o `AppId` no pipeline de inicialização. Permite identificar e configurar cada serviço de forma única em ambientes distribuídos.

## Utilização

Adicione o pacote e registre o AppId no início da configuração:

```csharp
builder.RegisterApp("MeuAppId");
```
