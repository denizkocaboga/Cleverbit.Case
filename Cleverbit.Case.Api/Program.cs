using Cleverbit.Case.Common.Exceptions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Net;

namespace Cleverbit.Case.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IServiceCollection services = builder.Services;

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddInfrastructures(builder.Configuration);

            services.AddBusinessServices();

            var app = builder.Build();

            app.UseExceptionHandler("/error");
            app.MapHealthChecks("/error/health");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }


}