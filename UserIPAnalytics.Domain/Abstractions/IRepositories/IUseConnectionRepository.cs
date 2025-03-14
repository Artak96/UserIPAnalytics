using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Domain.Abstractions.IRepositories
{
    public interface IUseConnectionRepository : IBaseRepository<UserConnection>
    {
        Task<List<long>> FindUsersByIpPartAsync(string ipAddress);

        Task<List<string>> GetAllUserIpsAsync(long userId);

       /// <summary>
       /// 
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
        Task<List<UserConnection>> GetUserIpAddressesAsync(long userId);
        Task<UserConnection?> GetLastConnectionAsync(long userId);
    }
}
