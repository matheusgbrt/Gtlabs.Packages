using Gtlabs.DependencyInjections.DependencyInjectons.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gtlabs.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork, ITransientDependency
{
    private readonly DbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public async Task<IUnitOfWorkTransaction> BeginAsync(CancellationToken cancellationToken = default)
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new UnitOfWorkTransaction(_context, _currentTransaction);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}