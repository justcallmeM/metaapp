namespace metaapp
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    using DataAccess.Behaviour;
    using DataAccess.Behaviour.Interfaces;
    using DataAccess.DataAccess;
    using DataAccess.DataAccess.Interfaces;
    using Infrastructure.Clients.WeatherAPI;
    using Infrastructure.Clients.WeatherAPI.Interfaces;
    using Interfaces;

    internal class Program
    {
        private static readonly IHost host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config => {
                config.AddJsonFile($"{Environment.CurrentDirectory}\\appsettings.json");
            })
            .UseSerilog((context, configuration) => {
                configuration
                    .ReadFrom.Configuration(context.Configuration);
            })
            .ConfigureServices(services => {
                services.AddSingleton<IApp, App>();
                services.AddSingleton<IWeatherDataBehaviour, WeatherDataBehaviour>();
                services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

                services.AddHttpClient<IWeatherApiWeatherClient, WeatherApiWeatherClient>();
            })
            .Build();

        static async Task Main(string[] args)
        {
            string arguments = string.Join(" ", args.Skip(1));

            await host.Services.GetRequiredService<IApp>().PeriodicWeatherDataRetrieval(arguments, TimeSpan.FromSeconds(30));
        }
    }
}