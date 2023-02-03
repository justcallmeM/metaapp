namespace metaapp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    using Infrastructure.Clients.WeatherAPI.Interfaces;
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;
    using DataAccess.Behaviour.Interfaces;
    using DataAccess.Models;
    using Interfaces;

    public class App : IApp
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IWeatherApiWeatherClient _weatherApiWeatherClient;
        private readonly IWeatherDataBehaviour _weatherDataBehaviour;
        private readonly ILogger _logger;

        public App(
            IHostApplicationLifetime applicationLifetime,
            IWeatherApiWeatherClient weatherApiWeatherClient,
            IWeatherDataBehaviour weatherDataBehaviour,
            ILogger logger)
        {
            _applicationLifetime = applicationLifetime;
            _weatherApiWeatherClient = weatherApiWeatherClient;
            _weatherDataBehaviour = weatherDataBehaviour;
            _logger = logger;
        }

        public async Task PeriodicWeatherDataRetrieval(string consoleArguments, TimeSpan interval)
        {
            List<WeatherRequest> requests = new();
            try
            {
                requests = CreateWeatherRequests(consoleArguments);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex.Message);
            }

            using var timer = new PeriodicTimer(interval);
            do
            {
                var weatherData = await GetAndSaveWeatherAsync(requests);
                DisplayWeatherData(weatherData);

            } while (await timer.WaitForNextTickAsync(_applicationLifetime.ApplicationStopping));
        }

        private async Task<IEnumerable<WeatherResponse>> GetAndSaveWeatherAsync(List<WeatherRequest> requests)
        {
            var weatherDataItems = await _weatherApiWeatherClient.GetWeatherAsync(requests);

            foreach (var forecast in weatherDataItems.GroupBy(x => x.Location.Name).Select(x => x.First()))
            {
                var weatherData = new WeatherData(
                        forecast.Location.Name,
                        forecast.Location.Country,
                        forecast.Weather.LastUpdated,
                        forecast.Weather.Temperature,
                        forecast.Weather.TemperatureFeelsLike,
                        forecast.Weather.WindSpeed,
                        forecast.Weather.WindDirection,
                        forecast.Weather.Precipitation,
                        forecast.Weather.Humidity,
                        forecast.Weather.CloudCoverage,
                        forecast.Weather.UltravioletIndex);

                await _weatherDataBehaviour.UpsertWeatherDataAsync(weatherData);
            }

            return weatherDataItems;
        }

        private static List<WeatherRequest> CreateWeatherRequests(string input)
        {
            List<WeatherRequest> requests = new();

            var requestParameters = CreateRequestParameters(input);

            foreach (var argument in requestParameters)
            {
                switch (argument.Key)
                {
                    case "city":
                        foreach (var city in argument.Value)
                        {
                            requests.Add(new WeatherRequest(city));
                        }
                        break;
                    default:
                        throw new ArgumentException("This argument isn't supported");
                }
            }

            return requests;

            static Dictionary<string, List<string>> CreateRequestParameters(string input)
            {
                Dictionary<string, List<string>> argumentAndValuePairs = new();

                foreach (var item in input.Split("--").Where(x => !string.IsNullOrEmpty(x)))
                {
                    var argument = item.Split(' ').First();
                    var value = item.Split(' ').Where(x => x != argument).Select(x => x.ToLower().Trim(',')).ToList();

                    argumentAndValuePairs.Add(argument, value);
                }

                return argumentAndValuePairs;
            }
        }

        private static void DisplayWeatherData(IEnumerable<WeatherResponse> weatherData)
        {
            if(!weatherData.Any())
            {
                Console.WriteLine("No data was retrieved");
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
