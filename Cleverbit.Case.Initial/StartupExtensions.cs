using Cleverbit.Case.Business.MappingProfiles;
using Cleverbit.Case.Business.Services;
using Cleverbit.Case.Infrastructure;
using Cleverbit.Case.Infrastructure.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cleverbit.Case.Initial
{
    public static class StartupExtensions
    {
        public static void AddInfrastructures(this IServiceCollection services, IConfiguration config)
        {
            services.AddCsvContextSingleton();
            services.AddSqlDb(config, ServiceLifetime.Singleton);
            services.AddCacheSingleton();
            
            services.AddAutoMapper(cfg => cfg.AddProfile<MapProfile>(),
                AppDomain.CurrentDomain.GetAssemblies(),
                ServiceLifetime.Singleton
                );
        }

        public static void AddBusinesServices(this IServiceCollection services)
        {            
            services.AddSingleton<ISqlRepository, SqlRepository>();
            services.AddSingleton<IInitialDataService, InitialDataService>();
        }
    }
}