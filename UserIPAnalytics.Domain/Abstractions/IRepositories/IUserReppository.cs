using UserIPAnalytics.Domain.Entities;

namespace UserIPAnalytics.Domain.Abstractions.IRepositories
{
    public interface IUserReppository : IBaseRepository<User>
    {
        Task<User?> GetUserByIdAsync(long Id);

    }
}
