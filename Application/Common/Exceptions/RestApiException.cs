using System.Net;

namespace Application.Common.Exceptions;

public class RestApiException : Exception
{
    public HttpStatusCode StatusCode { get; private set; }

    public RestApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message)
    {
        StatusCode = statusCode;
    }
}