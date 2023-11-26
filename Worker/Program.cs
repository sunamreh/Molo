using Molo.Worker.Consumers;
using Molo.Worker.Workers;
using Molo.Application;
using Molo.Infrastructure;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddOptions();
                    services.AddApplication();
                    services.AddInfrastructure(configuration);
                    services.AddHostedService<SubscriptionConsumer>();
                    services.AddHostedService<TransactionConsumer>();
                    services.AddHostedService<CollectConsumer>();
                })
                .Build();

            host.Run();
        }
    }
}