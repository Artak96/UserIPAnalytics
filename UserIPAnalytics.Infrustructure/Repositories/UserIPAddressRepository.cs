using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Entities;
using UserIPAnalytics.Infrustructure.Data.Context;

namespace UserIPAnalytics.Infrustructure.Repositories
{
    public class UserIPAddressRepository : BaseRepository<UserIPAddress>, IUserIPAddressRepository
    {
        public UserIPAddressRepository(UserIPTrackerDbContext context) : base(context)
        {
        }
    }
}
