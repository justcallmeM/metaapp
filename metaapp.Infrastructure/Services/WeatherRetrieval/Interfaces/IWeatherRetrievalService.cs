using metaapp.Application.Common.Models.Requests;
using metaapp.Application.Common.Models.Responses;

namespace metaapp.Infrastructure.Services.Weather.Interfaces
{
    public interface IWeatherRetrievalService
    {
        Task<IEnumerable<WeatherResponse>> GetAndSaveWeatherAsync(List<WeatherRequest> requests);
        List<WeatherRequest> CreateWeatherRequests(string input);
    }
}