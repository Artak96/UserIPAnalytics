using Microsoft.EntityFrameworkCore;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Domain.Common;
using UserIPAnalytics.Infrustructure.Data.Context;

namespace UserIPAnalytics.Infrustructure.UOW
{
    public class UnitOfWOrk : IUnitOfWork
    {
        private readonly UserIpAnalysticDbContext _dbContext;

        public UnitOfWOrk(UserIpAnalysticDbContext context)
        {
            _dbContext = context;
        }

        public IUseConnectionRepository UserIPAddress => throw new NotImplementedException();

        public IUserReppository User => throw new NotImplementedException();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
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
