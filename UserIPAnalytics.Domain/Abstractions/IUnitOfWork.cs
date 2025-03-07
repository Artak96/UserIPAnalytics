using UserIPAnalytics.Domain.Abstractions.IRepositories;

namespace UserIPAnalytics.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IUserIPAddressRepository UserIPAddress { get; }
        IUserReppository User { get; }

    }
}
