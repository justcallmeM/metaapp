namespace metaapp.Infrastructure.Clients.WeatherAPI
{
    using Application.Common.Constants;
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;
    using Interfaces;
    using Serilog;

    public class WeatherApiWeatherClient : BaseApiClient, IWeatherApiWeatherClient
    {
        public WeatherApiWeatherClient(
            HttpClient httpClient,
            ILogger logger)
            : base(httpClient, logger) { }

        public async Task<IEnumerable<WeatherResponse>> GetWeatherAsync(
            List<WeatherRequest> requests)
        {
            List<WeatherResponse> responses = new();

            foreach (var request in requests)
            {
                try
                {
                    responses.Add(await GetAsync<WeatherResponse>(ExternalClient.WeatherAPI.Weather.Current.Get(request)));
                }
                catch (ArgumentNullException ex)
                {
                    logger.Error($"An error occured while processing user request. Location: {ex.Message}, Exception: {ex.StackTrace}");
                }
            }

            return responses;
        }
    }
}
