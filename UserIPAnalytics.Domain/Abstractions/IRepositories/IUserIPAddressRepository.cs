using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Domain.Abstractions.IRepositories
{
    public interface IUserIPAddressRepository : IBaseRepository<UserIPAddress>
    {
        Task<List<long>> FindUsersByIpPartAsync(string ipPart);

        Task<List<string>> GetAllUserIpsAsync(long userId);

        Task<(string? IpAddress, DateTime? ConnectionTime)> GetLastUserConnectionAsync(long userId);
    }
}
