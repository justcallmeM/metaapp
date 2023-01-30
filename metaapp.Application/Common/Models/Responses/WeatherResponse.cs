namespace metaapp.Application.Common.Models.Responses
{
    using System.Text.Json.Serialization;

    public record WeatherResponse(
        [property: JsonPropertyName("location")] Location Location,
        [property: JsonPropertyName("current")] Weather Weather);

    public record Location(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("region")] string Region,
        [property: JsonPropertyName("country")] string Country,
        [property: JsonPropertyName("lat")] double Latitude,
        [property: JsonPropertyName("lon")] double Longitude,
        [property: JsonPropertyName("tz_id")] string TimezoneName);

    public record Weather(
        [property: JsonPropertyName("last_updated")] string LastUpdated,
        [property: JsonPropertyName("temp_c")] double Temperature,
        [property: JsonPropertyName("feelslike_c")] double TemperatureFeelsLike,
        [property: JsonPropertyName("wind_kph")] double WindSpeed,
        [property: JsonPropertyName("wind_dir")] string WindDirection,
        [property: JsonPropertyName("precip_mm")] double Precipitation,
        [property: JsonPropertyName("humidity")] int Humidity,
        [property: JsonPropertyName("cloud")] int CloudCoverage,
        [property: JsonPropertyName("uv")] double UltravioletIndex);
}
