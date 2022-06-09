using Microsoft.AspNetCore.Http;

namespace Camino.Infrastructure.Http.Interfaces
{
    public interface IHttpHelper
    {
        bool IsAjaxRequest(HttpRequest request);
    }
}
