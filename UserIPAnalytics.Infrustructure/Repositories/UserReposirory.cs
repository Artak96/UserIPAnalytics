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

        /// <summary>
        /// Найти пользователей по начальной или полной части IP-адреса
        /// </summary>
        public async Task<List<long>> FindUsersByIpPartAsync(string ipPart)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.IPAddress.StartsWith(ipPart))
                .Select(uc => uc.UserId)
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Найти все IP-адреса пользователя
        /// </summary>
        public async Task<List<string>> GetAllUserIpsAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.IPAddress  )
                .Distinct()
                .ToListAsync();
        }

        /// <summary>
        /// Найти время и IP последнего подключения пользователя
        /// </summary>
        public async Task<(string? IpAddress, DateTime? ConnectionTime)> GetLastUserConnectionAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .OrderByDescending(uc => uc.CreatedAt)
                .Select(uc => new { uc.IPAddress, uc.CreatedAt })
                .FirstOrDefaultAsync()
                is var result && result != null
                ? (result.IPAddress, result.CreatedAt)
                : (null, null);
        }
    }
}
