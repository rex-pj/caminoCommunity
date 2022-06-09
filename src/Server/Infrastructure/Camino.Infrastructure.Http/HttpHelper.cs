using Camino.Infrastructure.Http.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Camino.Infrastructure.Http
{
    public class HttpHelper : IHttpHelper
    {
        public bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Headers == null)
            {
                return false;
            }

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
