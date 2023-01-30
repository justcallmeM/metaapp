namespace metaapp.Infrastructure.Clients.WeatherAPI
{
    using metaapp.Application.Common.Exceptions;
    using Serilog;

    public abstract class WeatherApiBaseClient : BaseApiClient
    {
        private readonly ILogger _logger;
        protected WeatherApiBaseClient(
            HttpClient httpClient,
            ILogger logger) : base(httpClient)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> ExecuteRequestAsync(
            HttpMethod httpMethod, string uri, object? requestBody = null)
        {
            try
            {
                return await base.ExecuteRequestAsync(httpMethod, uri, requestBody);
            }
            catch (ExternalApiException ex)
            {
                _logger.Error(ex.ToString());

                throw ex;
            }
        }
    }
}
