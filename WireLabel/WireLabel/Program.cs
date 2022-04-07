using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WireLabel.Services;
using WireLabel.BL.Services;
using WireLabel.BL.Services.Interfaces;

namespace WireLabel;

public class Program
{
    static void Main(string[] args)
    {
        Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .ConfigureHostConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
            })
            .ConfigureServices(services => services.AddSingleton<WorkerService>())
            .Build()
            .Services
            .GetService<WorkerService>()
            ?.Start();
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
    }
}