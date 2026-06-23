using Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var entityType = typeof(T);

            if (!_repositories.TryGetValue(entityType, out var repositoryInstance))
            {
                repositoryInstance = new Repository<T>(_context);
                _repositories[entityType] = repositoryInstance;
            }

            return (IRepository<T>)repositoryInstance;
        }



        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            await _currentTransaction!.CommitAsync(ct);
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            await _currentTransaction!.RollbackAsync(ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }

            await _context.DisposeAsync();
        }
    }
}
