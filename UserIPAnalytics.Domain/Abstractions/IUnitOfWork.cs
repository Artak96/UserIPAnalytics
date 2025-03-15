using UserIPAnalytics.Domain.Abstractions.IRepositories;

namespace UserIPAnalytics.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IUseConnectionRepository UserConnection { get; }
        IUserReppository User { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
