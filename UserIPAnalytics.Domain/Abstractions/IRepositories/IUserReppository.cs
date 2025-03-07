using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Domain.Abstractions.IRepositories
{
    public interface IUserReppository : IBaseRepository<User>
    {
        Task<List<long>> FindUsersByIpPartAsync(string ipPart);

        /// <summary>
        /// Найти все IP-адреса пользователя
        /// </summary>
        Task<List<string>> GetAllUserIpsAsync(long userId);

        /// <summary>
        /// Найти время и IP последнего подключения пользователя
        /// </summary>
        Task<(string? IpAddress, DateTime? ConnectionTime)> GetLastUserConnectionAsync(long userId);
    }
}
