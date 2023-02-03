namespace metaapp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    using Infrastructure.Services.Weather.Interfaces;
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;
    using Interfaces;
    

    public class App : IApp
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IWeatherRetrievalService _weatherRetrievalService;
        private readonly ILogger _logger;

        public App(
            IHostApplicationLifetime applicationLifetime,
            IWeatherRetrievalService weatherRetrievalService,
            ILogger logger)
        {
            _applicationLifetime = applicationLifetime;
            _weatherRetrievalService = weatherRetrievalService;
            _logger = logger;
        }

        public async Task PeriodicWeatherDataRetrieval(string consoleArguments, TimeSpan interval)
        {
            List<WeatherRequest> requests = new();
            try
            {
                requests = _weatherRetrievalService.CreateWeatherRequests(consoleArguments);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex.Message);
            }

            using var timer = new PeriodicTimer(interval);
            do
            {
                var weatherData = await _weatherRetrievalService.GetAndSaveWeatherAsync(requests);
                DisplayWeatherData(weatherData);

            } while (await timer.WaitForNextTickAsync(_applicationLifetime.ApplicationStopping));
        }

        private static void DisplayWeatherData(IEnumerable<WeatherResponse> weatherData)
        {
            if(!weatherData.Any())
            {
                Console.WriteLine("No data was retrieved, application is going to continue working.");
            }

            foreach (WeatherResponse response in weatherData.ToList())
            {
                Console.WriteLine(
                    $"{response.Location.Country} - {response.Location.Name}: \n" +
                    $"  Temperature - {response.Weather.Temperature} \n" +
                    $"  FeelsLike   - {response.Weather.TemperatureFeelsLike} ...");
            }
        }
    }
}
