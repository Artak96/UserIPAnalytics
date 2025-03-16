using Microsoft.EntityFrameworkCore;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Common;
using UserIPAnalytics.Infrustructure.Data.Context;
using UserIPAnalytics.Infrustructure.Repositories;

namespace UserIPAnalytics.Infrustructure.UOW
{
    public class UnitOfWOrk : IUnitOfWork
    {
        private readonly UserIpAnalysticDbContext _dbContext;

        public UnitOfWOrk(UserIpAnalysticDbContext context)
        {
            _dbContext = context;
        }

        private UseConnectionRepository _userConnection;
        public IUseConnectionRepository UserConnection => _userConnection ?? new UseConnectionRepository(_dbContext);

        private UserReposirory _user;
        public IUserReppository User => _user ?? new UserReposirory(_dbContext);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (!_dbContext.ChangeTracker.HasChanges())
                return 0;

            var now = DateTime.UtcNow;

            foreach (var entry in _dbContext.ChangeTracker.Entries<Entity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = now;
                    entry.Entity.UpdatedDate = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.CreatedDate).IsModified = false;
                    entry.Entity.UpdatedDate = now;
                }
            }

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
