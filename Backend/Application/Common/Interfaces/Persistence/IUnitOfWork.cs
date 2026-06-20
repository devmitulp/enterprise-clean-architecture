namespace Application.Common.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;

        Task SaveChangesAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
