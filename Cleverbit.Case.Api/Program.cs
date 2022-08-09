using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cleverbit.Case.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var allowedOrigins = "allowedOrigins";

            var builder = WebApplication.CreateBuilder(args);

            IServiceCollection services = builder.Services;

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddInfrastructures(builder.Configuration);

            services.AddBusinessServices();

            services.AddCors(options =>
            {
                options.AddPolicy(allowedOrigins, policy => { policy.WithOrigins("https://localhost:7211", "http://localhost:5211"); });
            });

            var app = builder.Build();

            app.UseExceptionHandler("/error");
            app.MapHealthChecks("/healtz");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(allowedOrigins);

            app.MapControllers();

            app.Run();
        }
    }


}