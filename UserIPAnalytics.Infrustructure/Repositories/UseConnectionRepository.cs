using Microsoft.EntityFrameworkCore;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Entities;
using UserIPAnalytics.Infrustructure.Data.Context;

namespace UserIPAnalytics.Infrustructure.Repositories
{
    public class UseConnectionRepository : BaseRepository<UserConnection>, IUseConnectionRepository
    {
        public UseConnectionRepository(UserIpAnalysticDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Get data by ip addres
        /// </summary>
        /// <param name="ipPart"></param>
        /// <returns></returns>
        public async Task<List<UserConnection>> FindUsersByIpPartAsync(string ipAddress)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.IpAddress.StartsWith(ipAddress))
                .ToListAsync();
        }

        /// <summary>
        /// Get current user ip addres
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserAllIpsAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.IpAddress)
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Get user last connection
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserConnection?> GetUserLastConnectionAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .OrderByDescending(uc => uc.CreatedDate)
                .FirstOrDefaultAsync();
        }

    }
}
