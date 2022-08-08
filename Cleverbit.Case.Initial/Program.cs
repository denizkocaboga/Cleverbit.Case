using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cleverbit.Case.Initial
{

    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddInfrastructures(ctx.Configuration);
                    services.AddBusinesServices();
                })
                .Build();

            host.Run();
        }
    }
}