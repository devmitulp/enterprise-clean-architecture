using Application.Common.Interfaces.Persistence;
using Domain.Entities.Users;
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

        public IRepository<User> Users => Repository<User>();

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        
        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.SaveChangesAsync();
            await _currentTransaction!.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _currentTransaction!.RollbackAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }
        }
    }
}
