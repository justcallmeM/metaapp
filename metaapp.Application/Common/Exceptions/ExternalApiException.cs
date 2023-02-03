namespace metaapp.Application.Common.Exceptions
{
    using System.Net;
    using System.Text;

    public class ExternalApiException : ApplicationException
    {
        public ExternalApiException(
            HttpStatusCode statusCode,
            Uri calledUri,
            string? responseContent = null,
            object? requestBody = null)
        {
            StatusCode = statusCode;
            CalledUri = calledUri;
            ResponseContent = responseContent;
            RequestBody = requestBody;
        }

        public readonly HttpStatusCode StatusCode;

        public readonly Uri CalledUri;

        public readonly string? ResponseContent;

        public readonly object? RequestBody;

        public override string ToString()
        {
            var sb = new StringBuilder(
                typeof(ExternalApiException).FullName + ": " + Message
            );

            sb.AppendLine();
            sb.AppendLine("Data:");
            sb.AppendLine($"{nameof(StatusCode)}: {StatusCode} ({(int)StatusCode})");

            if (CalledUri is not null)
                sb.AppendLine($"{nameof(CalledUri)}: {CalledUri}");

            if (ResponseContent is not null)
                sb.AppendLine($"{nameof(ResponseContent)}: {ResponseContent}");

            if (RequestBody is not null)
                sb.AppendLine($"{nameof(RequestBody)}: {ResponseContent}");

            if (StackTrace is not null)
                sb.AppendLine(StackTrace);

            return sb.ToString();
        }
    }
}
