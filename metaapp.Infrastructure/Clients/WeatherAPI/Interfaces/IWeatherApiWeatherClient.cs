namespace metaapp.Infrastructure.Clients.WeatherAPI.Interfaces
{
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;

    public interface IWeatherApiWeatherClient
    {
        Task<IEnumerable<WeatherResponse>> GetWeatherAsync(List<WeatherRequest> requests);
    }
}
