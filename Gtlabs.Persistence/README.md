# Gtlabs.Persistence

Biblioteca de persistência para aplicações .NET, fornecendo integrações e convenções sobre Entity Framework Core com padrões Repository e Unit of Work. Inclui entidades base (auditoria, soft-delete), um DbContext customizado, extensões para migração e um serviço para aplicar migrações no startup.

## Estrutura (resumo)
- CustomDbContext/: implementação do DbContext utilizada pela aplicação.
- Entities/: entidades base (Entity, AuditedEntity, SoftDeleteEntity).
- Extensions/: helpers para configuração e migração de banco.
- Repository/: IRepository, Repository e UnitOfWork.
- Services/: serviço para aplicar migrações automaticamente.

## Caso de uso: CustomDbContext
Use o `GtLabsDbContext` para centralizar a configuração do modelo, convenções e interceptores/companhias de auditoria antes de usar os repositórios ou o EF diretamente.

Exemplo de uso:
```csharp
// registrar no DI
services.AddDbContext<GtLabsDbContext>(opts => 
    opts.UseSqlServer(configuration.GetConnectionString("Default")));

// injetar e usar
public class MyAppService
{
    private readonly GtLabsDbContext _db;
    public MyAppService(GtLabsDbContext db) => _db = db;

    public void Add<T>(T entity) where T : class
    {
        _db.Set<T>().Add(entity);
        _db.SaveChanges();
    }
}
```

## Caso de uso: Repository
Use o padrão Repository para isolar a lógica de acesso a dados da camada de aplicação, facilitando testes e mantendo regras de consulta centralizadas.

Exemplo de uso:
```csharp
public class MyService
{
    private readonly IRepository<MyEntity> _repo;
    private readonly IUnitOfWork _uow;

    public MyService(IRepository<MyEntity> repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public IEnumerable<MyEntity> ListActive()
    {
        return _repo.Where(e => !e.IsDeleted).ToList();
    }

    public void Create(MyEntity e)
    {
        _repo.Add(e);
        _uow.SaveChanges();
    }
}
```

## Instalação
Adicione o projeto como referência ou publique como pacote NuGet e referencie no seu projeto.

## Licença
Verifique o arquivo LICENSE no repositório principal.