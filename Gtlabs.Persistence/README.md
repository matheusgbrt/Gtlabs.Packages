# Gtlabs.Persistence

Persistence library for .NET applications that provides integrations and conventions on top of Entity Framework Core using Repository and Unit of Work patterns. Includes base entities (auditing, soft-delete), a custom DbContext, migration extensions and a service to apply migrations at startup.

## Structure (summary)
- CustomDbContext/: custom `DbContext` implementation used by the application.
- Entities/: base entities (`Entity`, `AuditedEntity`, `SoftDeleteEntity`).
- Extensions/: helpers for database configuration and migrations.
- Repository/: `IRepository`, `Repository` and `UnitOfWork`.
- Services/: service to apply migrations automatically.

## Use case: CustomDbContext
Use `GtLabsDbContext` to centralize model configuration, conventions and auditing/interceptors before using repositories or EF directly.

Example:
```csharp
// register in DI
services.AddDbContext<GtLabsDbContext>(opts =>
    opts.UseSqlServer(configuration.GetConnectionString("Default")));

// inject and use
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

## Use case: Repository
Use the Repository pattern to isolate data access logic from the application layer, simplifying tests and centralizing query rules.

Example:
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

## Installation
Add the project reference or publish as a NuGet package and reference it in your project.

## License
See the LICENSE file in the root repository for details.