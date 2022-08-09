using Cleverbit.Case.Infrastructure.Cache;
using Cleverbit.Case.Infrastructure.Csv;
using Cleverbit.Case.Infrastructure.SqlServer;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Linq;

namespace Cleverbit.Case.Infrastructure
{
    public static class StartupExtensions
    {
        public static void AddSqlDb(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifeTime = ServiceLifetime.Scoped)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SqlContext>(opts =>
                opts.UseSqlServer(connectionString, opt => opt.EnableRetryOnFailure()), lifeTime);
        }

        public static void AddCache(this IServiceCollection services)
        {
            services.AddScoped(p =>
            {
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(CacheConfig);
                return multiplexer.GetDatabase();
            });

            services.AddScoped<ICacheRepository, RedisRepository>();
        }

        //ToDo: Move Config to appsettings.

        private static string CacheConfig => "127.0.0.1,AllowAdmin=true";
        //This redis active and ready for use on azure.
        //private static string CacheConfig => "clbredis.redis.cache.windows.net,AllowAdmin=true,password=7ACqzeWnmIQFsZbFVdaUmDFVM7pQtnbmsAzCaKzQl6k=";

        public static void AddCacheSingleton(this IServiceCollection services)
        {
            services.AddSingleton(p =>
            {
                //ToDo: Inject IConnectionMultiplexer and get database in repository.
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(CacheConfig);
                multiplexer.GetServer(multiplexer.GetEndPoints().FirstOrDefault()).FlushAllDatabases();//ToDo: Move this in repository
                IDatabase result = multiplexer.GetDatabase();

                return result;
            });

            services.AddSingleton<ICacheInitialRepository, RedisInitialRepository>();
        }

        public static void AddCsvContext(this IServiceCollection services)
        {
            services.AddScoped<IFactory, Factory>();
            services.AddScoped<ICsvContext, CsvHelperContext>();
        }
        public static void AddCsvContextSingleton(this IServiceCollection services)
        {
            services.AddSingleton<IFactory, Factory>();
            services.AddSingleton<ICsvContext, CsvHelperContext>();
        }
    }
}