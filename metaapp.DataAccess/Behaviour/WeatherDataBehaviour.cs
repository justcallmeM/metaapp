namespace metaapp.DataAccess.Behaviour
{
    using Behaviour.Interfaces;
    using DataAccess.Interfaces;
    using Models.Constants;
    using Models;

    public class WeatherDataBehaviour : IWeatherDataBehaviour
    {
        private readonly ISqlDataAccess _database;

        public WeatherDataBehaviour(
            ISqlDataAccess database)
        {
            _database = database;
        }

        public async Task<IEnumerable<string>> GetAllLocations()
            => await _database.LoadDataAsync<string>(StoredProcedure.GetAllLocations);

        public async Task<IEnumerable<WeatherData>> GetWeatherDataByOneOrMoreCityNameAsync(string commaSeparatedCityNames)
            => await _database.LoadDataAsync<WeatherData, dynamic>(StoredProcedure.GetWeatherByCityName, new { CityNames = commaSeparatedCityNames });

        public async Task UpsertWeatherDataAsync(WeatherData weatherData)
            => await _database.SaveDataAsync(StoredProcedure.WeatherUpsert, weatherData);
    }
}
