using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Todo.Models
{
    public class ApplicationResult<T> : IApplicationResult<T>
    {
        public T Resource { get; }
        public HttpStatusCode StatusCode { get; }
        public IEnumerable<string> ErrorMessages { get; }

        private ApplicationResult(T resource)
        {
            if (resource is null)
                throw new ArgumentNullException(nameof(resource));
            
            Resource = resource;
            StatusCode = HttpStatusCode.OK;
            ErrorMessages = Enumerable.Empty<string>();
        }

        private ApplicationResult(IEnumerable<string> errorMessages, HttpStatusCode statusCode)
        {
            errorMessages ??= Enumerable.Empty<string>();

            Resource = default;
            StatusCode = statusCode;
        }

        public static IApplicationResult<T> Success<T>(T resource)
        {
            return new ApplicationResult<T>(resource);
        }

        public static IApplicationResult<T> FromError(IEnumerable<string> errorMessages, HttpStatusCode statusCode)
        {
            return new ApplicationResult<T>(errorMessages, statusCode);
        }
        
        public static IApplicationResult<T> FromError(string errorMessage, HttpStatusCode statusCode)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));
            
            var errorMessages = new [] { errorMessage }.AsEnumerable();
            
            return new ApplicationResult<T>(errorMessages, statusCode);
        }
    }
}