
namespace DDDMart.SharedKernel
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
