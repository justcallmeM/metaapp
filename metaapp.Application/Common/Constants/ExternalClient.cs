namespace metaapp.Application.Common.Constants
{
    using Models.Requests;

    public static class ExternalClient
    {
        public static class WeatherAPI
        {
            public static class Authorization
            {
                public const string Token = "fd567082f30c44099db214237221612";
            }

            public static class Weather
            {
                public const string BaseUri = "http://api.weatherapi.com/v1";

                public static class Current
                {
                    public const string Uri = BaseUri + $"/current.json?key={Authorization.Token}";

                    public static string Get(WeatherRequest request)
                        => $"{Uri}&q={request.CityName}";
                }
            }
        }
    }
}
