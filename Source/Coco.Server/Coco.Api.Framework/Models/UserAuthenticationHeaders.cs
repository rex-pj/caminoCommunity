namespace Coco.Api.Framework.Models
{
    public class UserAuthenticationHeaders
    {
        public string AuthenticationToken { get; set; }
        public string UserIdHashed { get; set; }
    }
}
