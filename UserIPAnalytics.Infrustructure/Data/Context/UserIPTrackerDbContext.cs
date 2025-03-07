using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserIPAnalytics.Domain.Entities;
using UserIPAnalytics.Infrustructure.Data.Configurations;

namespace UserIPAnalytics.Infrustructure.Data.Context
{
    public class UserIPTrackerDbContext : DbContext
    {
        public UserIPTrackerDbContext(DbContextOptions<UserIPTrackerDbContext> options) : base(options)
        {
        }

        public UserIPTrackerDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserIPAddress> UserIPAddress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
                var entityConfigurationsAssembly = typeof(UserConfiguration).GetTypeInfo().Assembly;
                modelBuilder.ApplyConfigurationsFromAssembly(entityConfigurationsAssembly);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException!.ToString());
            }
        }
    }
}
