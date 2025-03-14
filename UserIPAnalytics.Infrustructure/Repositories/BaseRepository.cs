using MediatR;
using Microsoft.EntityFrameworkCore;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Common;
using UserIPAnalytics.Infrustructure.Data.Context;

namespace UserIPAnalytics.Infrustructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly UserIpAnalysticDbContext _context;

        public BaseRepository(UserIpAnalysticDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

    }
}
