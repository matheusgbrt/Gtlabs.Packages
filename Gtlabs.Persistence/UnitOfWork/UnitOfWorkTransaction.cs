using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Gtlabs.Persistence.UnitOfWork;

public sealed class UnitOfWorkTransaction : IUnitOfWorkTransaction
{
    private readonly DbContext _context;
    private readonly IDbContextTransaction _transaction;
    private bool _disposed;
    private bool _committed;

    public UnitOfWorkTransaction(DbContext context, IDbContextTransaction transaction)
    {
        _context = context;
        _transaction = transaction;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
        _committed = true;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        if (!_committed)
        {
            await _transaction.RollbackAsync();
        }
        await _transaction.DisposeAsync();
        _disposed = true;
    }
}