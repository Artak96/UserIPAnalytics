using Microsoft.EntityFrameworkCore;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Entities;
using UserIPAnalytics.Infrustructure.Data.Context;

namespace UserIPAnalytics.Infrustructure.Repositories
{
    public class UserReposirory : BaseRepository<User>, IUserReppository
    {
        public UserReposirory(UserIPTrackerDbContext context) : base(context)
        {
        }

    }
}
