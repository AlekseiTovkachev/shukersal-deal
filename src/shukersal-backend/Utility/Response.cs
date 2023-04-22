﻿
using System.Net;

namespace shukersal_backend.Utility
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T? Result { get; set; }
        public string? ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public static Response<T> Success(HttpStatusCode statusCode, T result)
        {
            return new Response<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Result = result
            };
        }

        public static Response<T> Error(HttpStatusCode statusCode, string errorMessage)
        {
            return new Response<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };
        }

        public Response()
        {

        }
    }
}
