using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VectorSearchAiAssistant.Service.Models.ConfigurationOptions;

namespace DataLoader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddOptions<PostgreSQLSettings>().Bind(config.GetSection("MSPostgreSQLOpenAI:PostgreSQL"));
                services.AddSingleton<DataLoader>();
            });

            // Let the builder know we're running in a console
            builder.UseConsoleLifetime();

            // Add services to the container
            IHost host = builder.Build();

            DataLoader p = host.Services.GetService<DataLoader>();
            p.ApiUrl = config.GetSection("MSPostgreSQLOpenAI:PostgreSQL:DataLoader").GetValue<string>("ApiUrl");
            p.LoadData().Wait();

            host.Run();
        }
    }
}