using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserIPAnalytics.Domain.Abstractions;
using UserIPAnalytics.Domain.Abstractions.IRepositories;
using UserIPAnalytics.Infrustructure.Data.Context;
using UserIPAnalytics.Infrustructure.Repositories;
using UserIPAnalytics.Infrustructure.UOW;

namespace UserIPAnalytics.Infrustructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWOrk>();
            //services.AddScoped<IUserReppository, UserReposirory>();
            services.AddScoped<IUseConnectionRepository, UseConnectionRepository>();

            string connectionString = configuration.GetConnectionString("ConnectionString")!;
            services.AddDbContext<UserIpAnalysticDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging(false);
            }, ServiceLifetime.Scoped);
            return services;
        }
    }
}
