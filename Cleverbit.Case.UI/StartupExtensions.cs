using Cleverbit.Case.Business.MappingProfiles;
using Cleverbit.Case.Business.Services;
using Cleverbit.Case.Infrastructure;
using Cleverbit.Case.Infrastructure.SqlServer;

namespace Cleverbit.Case.UI
{
    public static class StartupExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<ISqlRepository, SqlRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ISearchRegionService, SearchRegionService>();
            services.AddScoped<IRegionService, RegionService>();
        }

        public static void AddInfrastructures(this IServiceCollection services, IConfiguration config)
        {
            services.AddSqlDb(config);
            services.AddCache();
            services.AddAutoMapper(cfg => cfg.AddProfile<MapProfile>());            
            
        }
    }


}