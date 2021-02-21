using Microsoft.AspNetCore.Http;

namespace Camino.Core.Contracts.Helpers
{
    public interface IHttpHelper
    {
        bool IsAjaxRequest(HttpRequest request);
    }
}
