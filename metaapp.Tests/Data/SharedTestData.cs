namespace metaapp.Tests.Data
{
    using AutoFixture;
    using Application.Common.Models.Requests;
    using Application.Common.Models.Responses;

    public class SharedTestData
    {
        public static List<WeatherResponse> GetWeatherResponseCollectionWithRequiredData(Fixture fixture)
        {
            return new List<WeatherResponse>()
            {
                new WeatherResponse(
                    new Location(
                        Name: fixture.Create<string>(),
                        Region: fixture.Create<string>(),
                        Country: fixture.Create<string>(),
                        Latitude: fixture.Create<double>(),
                        Longitude: fixture.Create<double>(),
                        TimezoneName: fixture.Create<string>()),
                    new Weather(
                        LastUpdated: fixture.Create<string>(),
                        Temperature: fixture.Create<double>(),
                        TemperatureFeelsLike: fixture.Create<double>(),
                        WindSpeed: fixture.Create<double>(),
                        WindDirection: fixture.Create<string>(),
                        Precipitation: fixture.Create<double>(),
                        Humidity: fixture.Create<int>(),
                        CloudCoverage: fixture.Create<int>(),
                        UltravioletIndex: fixture.Create<double>()))
            };
        }

        public static List<WeatherRequest> GetWeatherRequestCollectioNWithRequiredData(Fixture fixture)
        {
            return new List<WeatherRequest>()
            {
                new WeatherRequest(
                    CityName: fixture.Create<string>())
            };
        }
    }
}
