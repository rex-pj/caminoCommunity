using Camino.Core.Contracts.Helpers;
using Microsoft.AspNetCore.Http;
using System;

namespace Camino.Framework.Helpers
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
