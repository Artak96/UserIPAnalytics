using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Domain.Abstractions.IRepositories
{
    public interface IUseConnectionRepository : IBaseRepository<UserConnection>
    {
        Task<List<UserConnection>> FindUsersByIpPartAsync(string ipAddress);

        Task<List<string>> GetUserAllIpsAsync(long userId);
        Task<UserConnection?> GetUserLastConnectionAsync(long userId);

    }
}
