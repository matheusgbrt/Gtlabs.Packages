using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Gtlabs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gtlabs.Persistence.Repository;

public class Repository<T> : IRepository<T> where T : AuditedEntity, IScopedDependency
{
    protected readonly DbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(DbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = Context.Set<T>();
    }
    
    public IQueryable<T> Query() => DbSet.AsQueryable();

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task SaveAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var entry = Context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            await DbSet.AddAsync(entity);
        }

        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
    public async Task PurgeAsync(T entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}