using Gtlabs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gtlabs.Persistence.Repository;

public class Repository<T> : IRepository<T> where T : AuditedEntity
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;
    public DbContext Context => _context;
    public Repository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }
    
    public IQueryable<T> Query()
        => _dbSet;

    public async Task<T?> GetByIdAsync(Guid id, bool asNoTracking = true)
    {
        var dbset = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
        return await dbset.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true)
    {
        var dbset = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
        return await dbset.ToListAsync();
    }
    
    public async Task InsertAsync(T entity,bool autoSave = false)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        
        if(autoSave)
            await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(T entity, bool autoSave = false)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        entry.State = EntityState.Modified;
        if(autoSave)
            await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity, bool autoSave = false)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Remove(entity);
        if(autoSave)
            await _context.SaveChangesAsync();
    }
    
    public void SetEntityState(object entity, EntityState state)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Entry(entity).State = state;
    }
    
    public async Task SetEntityStateAsync(object entity, EntityState state, bool autoSave = false)
    {
        SetEntityState(entity, state);

        if (autoSave)
            await _context.SaveChangesAsync();
    }

}