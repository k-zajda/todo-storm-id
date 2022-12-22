using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Todo.Models
{
    public interface IApplicationResult<out T>
    {
        T Resource { get; }

        HttpStatusCode StatusCode { get; }
        
        IEnumerable<string> ErrorMessages { get; }

        public bool IsSuccess => ErrorMessages is null || ErrorMessages.Count() == 1;
    }
}