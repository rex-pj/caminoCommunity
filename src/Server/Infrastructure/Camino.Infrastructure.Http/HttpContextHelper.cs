using Microsoft.AspNetCore.Http;
using System.Net;

namespace Camino.Infrastructure.Http
{
    public class HttpContextHelper
    {
        public IPAddress GetRemoteIPAddress(HttpContext context, bool isAllowForwarded = true)
        {
            if (isAllowForwarded)
            {
                var header = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip;
                }
            }

            return context.Connection.RemoteIpAddress;
        }
    }
}
