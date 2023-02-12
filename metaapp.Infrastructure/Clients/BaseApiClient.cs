namespace metaapp.Infrastructure.Clients
{
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Text;
    using Application.Common.Constants;
    using Application.Common.Exceptions;
    using Serilog;

    public abstract class BaseApiClient
    {
        protected readonly HttpClient httpClient;
        protected readonly ILogger logger;

        protected BaseApiClient(
            HttpClient httpClient,
            ILogger logger)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(15);

            this.httpClient = httpClient;
            this.logger = logger;
        }

        protected virtual async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            HttpResponseMessage? responseMessage = null;

            try
            {
                responseMessage = await ExecuteRequestAsync(HttpMethod.Get, uri);
            }
            catch (ExternalApiException ex)
            {
                logger.Error(ex.ToString());
            }

            var stream = responseMessage?.Content.ReadAsStream();

            if(stream is not null)
            {
                var result = await JsonSerializer.DeserializeAsync<TResponse>(stream);
                return result!;
            }

            throw new ArgumentNullException(nameof(stream), $"of {nameof(responseMessage)} - content");
        }

        protected virtual async Task<TResponse> PostAsync<TRequest, TResponse>(
            string uri, TRequest request)
            where TRequest : class
        {
            HttpResponseMessage? responseMessage = null;

            try
            {
                responseMessage = await ExecuteRequestAsync(HttpMethod.Post, uri, request);
            }
            catch (ExternalApiException ex)
            {
                logger.Error(ex.ToString());
            }

            var stream = responseMessage?.Content.ReadAsStream();

            if (stream is not null)
            {
                var result = await JsonSerializer.DeserializeAsync<TResponse>(stream);
                return result!;
            }

            throw new ArgumentNullException(nameof(stream), $"of {nameof(responseMessage)} - content");
        }

        protected virtual async Task<TResponse> PatchAsync<TRequest, TResponse>(
            string uri, TRequest request)
            where TRequest : class
        {
            HttpResponseMessage? responseMessage = null;

            try
            {
                responseMessage = await ExecuteRequestAsync(HttpMethod.Patch, uri, request);
            }
            catch (ExternalApiException ex)
            {
                logger.Error(ex.ToString());
            }

            var stream = responseMessage?.Content.ReadAsStream();

            if (stream is not null)
            {
                var result = await JsonSerializer.DeserializeAsync<TResponse>(stream);
                return result!;
            }

            throw new ArgumentNullException(nameof(stream), $"of {nameof(responseMessage)} - content");
        }

        protected virtual async Task DeleteAsync(string uri)
        {
            try
            {
                await ExecuteRequestAsync(HttpMethod.Delete, uri);
            }
            catch (ExternalApiException ex)
            {
                logger.Error(ex.ToString());
            }
        }

        protected virtual async Task<HttpResponseMessage> ExecuteRequestAsync(
            HttpMethod httpMethod, string uri, object? requestBody = null)
        {
            var httpRequest = new HttpRequestMessage(httpMethod, uri);

            await AddRequestHeadersAsync(httpRequest.Headers);

            if (requestBody is not null)
            {
                httpRequest.Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    CommonConstant.MediaTypes.Json);
            }

            var response = await httpClient.SendAsync(httpRequest);

            await EnsureSuccessAsync(response, requestBody);

            return response;
        }

        protected virtual Task AddRequestHeadersAsync(
            HttpRequestHeaders headers)
        {
            headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue(CommonConstant.MediaTypes.Json));

            return Task.CompletedTask;
        }

        protected virtual async Task EnsureSuccessAsync(
            HttpResponseMessage response, object? requestBody)
        {
            if (response.IsSuccessStatusCode)
                return;

            var responseContent = response.Content is null
                ? null
                : await response.Content.ReadAsStringAsync();

            throw new ExternalApiException(
                response.StatusCode,
                response.RequestMessage!.RequestUri!,
                responseContent,
                requestBody
            );
        }
    }
}
