namespace metaapp.Infrastructure.Clients
{
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Text;
    using Application.Common.Constants;
    using Application.Common.Exceptions;

    public abstract class BaseApiClient
    {
        protected readonly HttpClient HttpClient;

        protected BaseApiClient(
            HttpClient httpClient)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(15);

            HttpClient = httpClient;
        }

        protected virtual async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            var response = await ExecuteRequestAsync(HttpMethod.Get, uri);

            return await JsonSerializer.DeserializeAsync<TResponse>(
                response.Content.ReadAsStream());
        }

        protected virtual async Task<TResponse> PostAsync<TRequest, TResponse>(
            string uri, TRequest request)
            where TRequest : class
        {
            var response = await ExecuteRequestAsync(HttpMethod.Post, uri, request);

            //throw an exception here. 
            //log as well.
            return await JsonSerializer.DeserializeAsync<TResponse>(
                response.Content.ReadAsStream());
        }

        protected virtual async Task<TResponse> PatchAsync<TRequest, TResponse>(
            string uri, TRequest request)
            where TRequest : class
        {
            var response = await ExecuteRequestAsync(HttpMethod.Patch, uri, request);

            return await JsonSerializer.DeserializeAsync<TResponse>(
                response.Content.ReadAsStream());
        }

        protected virtual Task Delete(string uri)
        {
            return ExecuteRequestAsync(HttpMethod.Delete, uri);
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

            var response = await HttpClient.SendAsync(httpRequest);

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
                response.RequestMessage.RequestUri,
                responseContent,
                requestBody
            );
        }
    }
}
