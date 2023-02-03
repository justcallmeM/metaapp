namespace metaapp.Infrastructure.Services.WeatherRetrieval
{
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;
    using DataAccess.Behaviour.Interfaces;
    using DataAccess.Models;
    using Clients.WeatherAPI.Interfaces;
    using Services.Weather.Interfaces;

    public class WeatherRetrievalService : IWeatherRetrievalService
    {
        private readonly IWeatherApiWeatherClient _weatherApiWeatherClient;
        private readonly IWeatherDataBehaviour _weatherDataBehaviour;

        public WeatherRetrievalService(
            IWeatherApiWeatherClient weatherApiWeatherClient,
            IWeatherDataBehaviour weatherDataBehaviour)
        {
            _weatherApiWeatherClient = weatherApiWeatherClient;
            _weatherDataBehaviour = weatherDataBehaviour;
        }

        public async Task<IEnumerable<WeatherResponse>> GetAndSaveWeatherAsync(List<WeatherRequest> requests)
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

        public List<WeatherRequest> CreateWeatherRequests(string input)
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
    }
}
