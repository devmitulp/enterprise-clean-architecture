namespace Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;

        Task SaveChangesAsync(CancellationToken ct = default);

        Task BeginTransactionAsync(CancellationToken ct = default);

        Task CommitTransactionAsync(CancellationToken ct = default);

        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
