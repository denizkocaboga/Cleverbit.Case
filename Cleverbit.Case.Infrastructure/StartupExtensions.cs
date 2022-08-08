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
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("clbredis.redis.cache.windows.net,password=7ACqzeWnmIQFsZbFVdaUmDFVM7pQtnbmsAzCaKzQl6k=");
                return multiplexer.GetDatabase();
            });

            services.AddScoped<ICacheRepository, RedisRepository>();
        }

        public static void AddCacheSingleton(this IServiceCollection services)
        {
            services.AddSingleton(p =>
            {
                //ToDo: Move Config to appsettings.
                //ToDo: Inject IConnectionMultiplexer and get database in repository.
                string redisConfig = "clbredis.redis.cache.windows.net,AllowAdmin=true,password=7ACqzeWnmIQFsZbFVdaUmDFVM7pQtnbmsAzCaKzQl6k=";
                IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(redisConfig);
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