using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Abstractions.IRepositories;

namespace UserIPAnalytics.Infrustructure.UOW
{
    public class UnitOfWOrk : IUnitOfWork
    {
        public IUserIPAddressRepository UserIPAddress => throw new NotImplementedException();

        public IUserReppository User => throw new NotImplementedException();
    }
}
