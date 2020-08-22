using Microsoft.AspNetCore.Http;

namespace Camino.Framework.Helpers.Contracts
{
    public interface IHttpHelper
    {
        bool IsAjaxRequest(HttpRequest request);
    }
}
