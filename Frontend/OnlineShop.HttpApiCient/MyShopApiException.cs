using OnlineShop.HttpModels.Responses;
using System.Net;
using System.Runtime.Serialization;

namespace OnlineShop.HttpApiCient
{
    [Serializable]
    public class MyShopApiException : Exception
    {
        public HttpStatusCode? StatusCode { get; }
        public ValidationProblemDetails? Details { get; }
        public ErrorResponse? Error { get;}

        public MyShopApiException()
        {
        }

        public MyShopApiException(HttpStatusCode statusCode, ValidationProblemDetails details) : base(details.Title)
        {
            StatusCode = statusCode;
            Details = details;
        }
        public MyShopApiException(ErrorResponse error) : base(error.Message)
        {
            Error = error;
            StatusCode = error.StatusCode!;
        }

        public MyShopApiException(string? message) : base(message)
        {
        }

        public MyShopApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}