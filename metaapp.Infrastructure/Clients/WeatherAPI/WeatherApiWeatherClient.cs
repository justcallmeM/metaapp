namespace metaapp.Infrastructure.Clients.WeatherAPI
{
    using Application.Common.Constants;
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;
    using Interfaces;
    using Serilog;

    public class WeatherApiWeatherClient : WeatherApiBaseClient, IWeatherApiWeatherClient
    {
        public WeatherApiWeatherClient(
            HttpClient httpClient,
            ILogger logger)
            : base(httpClient, logger) { }

        public async Task<IEnumerable<WeatherResponse>> GetWeatherAsync(
            List<WeatherRequest> requests)
        {
            List<WeatherResponse> weatherData = new();

            foreach (var request in requests)
            {
                weatherData.Add(
                    await GetAsync<WeatherResponse>(ExternalClient.WeatherAPI.Weather.Current.Get(request))
                );
            }

            return weatherData;
        }
    }
}
