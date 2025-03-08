using Microsoft.EntityFrameworkCore;
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

        public async Task<List<long>> FindUsersByIpPartAsync(string ipPart)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.IPAddress.StartsWith(ipPart))
                .Select(uc => uc.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetAllUserIpsAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.IPAddress)
                .Distinct()
                .ToListAsync();
        }

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
