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

        public async Task<List<long>> FindUsersByIpPartAsync(string ipPart)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.IpAddress.StartsWith(ipPart))
                .Select(uc => uc.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetAllUserIpsAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.IpAddress)
                .Distinct()
                .ToListAsync();
        }

        public Task<UserConnection?> GetLastConnectionAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(string? IpAddress, DateTime? ConnectionTime)> GetLastUserConnectionAsync(long userId)
        {
            return await _context.UserIPAddress
                .Where(uc => uc.UserId == userId)
                .OrderByDescending(uc => uc.CreatedDate)
                .Select(uc => new { uc.IpAddress, uc.CreatedDate })
                .FirstOrDefaultAsync()
                is var result && result != null
                ? (result.IpAddress, result.CreatedDate)
                : (null, null);
        }

        public Task<List<string>> GetUserIpAddressesAsync(long userId)
        {
            throw new NotImplementedException();
        }

        Task<List<UserConnection>> IUseConnectionRepository.GetUserIpAddressesAsync(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
