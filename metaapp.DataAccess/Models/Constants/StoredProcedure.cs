namespace metaapp.DataAccess.Models.Constants
{
    public static class StoredProcedure
    {
        public const string GetAllLocations = "dbo.Location_GetAll";
        public const string GetWeatherByCityName = "dbo.Weather_GetByCityName";
        public const string WeatherUpsert = "dbo.Weather_Upsert";
    }
}
