using Microsoft.EntityFrameworkCore;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Entities;
using UserIPAnalytics.Infrustructure.Data.Context;

namespace UserIPAnalytics.Infrustructure.Repositories
{
    public class UserReposirory : BaseRepository<User>, IUserReppository
    {
        public UserReposirory(UserIpAnalysticDbContext context) : base(context)
        {
        }

        public Task<List<long>> FindUserIdsByIpPrefixAsync(string ipPrefix)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByIdAsync(long Id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
        }


    }
}
