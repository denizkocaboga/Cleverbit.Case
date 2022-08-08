using Cleverbit.Case.Business.MappingProfiles;
using Cleverbit.Case.Business.Services;
using Cleverbit.Case.Common.Exceptions;
using Cleverbit.Case.Infrastructure;
using Cleverbit.Case.Infrastructure.SqlServer;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net;

namespace Cleverbit.Case.Api
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
            services.AddSwaggerGen();
            services.AddCsvContext();
            services.AddSqlDb(config);
            services.AddCache();
            services.AddAutoMapper(cfg => cfg.AddProfile<MapProfile>());
            services.AddFluentValidationAutoValidation();
            services.AddHealthChecks();            
        }
    }


}