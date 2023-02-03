namespace metaapp.DataAccess.Models
{
    public record WeatherData(
        string CityName,
        string CountryName,
        string LastUpdated,
        double Temperature,
        double TemperatureFeelsLike,
        double WindSpeed,
        string WindDirection,
        double Precipitation,
        int Humidity,
        int CloudCoverage,
        double UltravioletIndex)
    {
        public WeatherData() 
            : this(default!, default!, default!, default, default, default, default!, default, default, default, default) 
        { }
    }
}
