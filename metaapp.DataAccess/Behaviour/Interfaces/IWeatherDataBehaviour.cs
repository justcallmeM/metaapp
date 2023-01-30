namespace metaapp.DataAccess.Behaviour.Interfaces
{
    using Models;

    public interface IWeatherDataBehaviour
    {
        Task<IEnumerable<string>> GetAllLocations();
        Task<IEnumerable<WeatherData>> GetWeatherDataByOneOrMoreCityNameAsync(string commaSeparatedCityNames);
        Task UpsertWeatherDataAsync(WeatherData weatherData);
    }
}
