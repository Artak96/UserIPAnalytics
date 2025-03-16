using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserIPAnalytics.Domain.Entities;
using UserIPAnalytics.Infrustructure.Data.Configurations;

namespace UserIPAnalytics.Infrustructure.Data.Context
{
    public class UserIpAnalysticDbContext : DbContext
    {
        public UserIpAnalysticDbContext(DbContextOptions<UserIpAnalysticDbContext> options) : base(options)
        {
        }

        public UserIpAnalysticDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserConnection> UserIPAddress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
                var entityConfigurationsAssembly = typeof(UserConnectionConfiguration).GetTypeInfo().Assembly;
                modelBuilder.ApplyConfigurationsFromAssembly(entityConfigurationsAssembly);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException!.ToString());
            }
        }
    }
}
