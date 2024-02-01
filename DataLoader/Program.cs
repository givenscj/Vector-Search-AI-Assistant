using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DataLoader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.AddEnvironmentVariables();



            using IHost host = builder.Build();

            host.Run();
        }
    }
}