namespace Camino.Shared.Requests.Authentication
{
    public class UserLoginRequest
    {
        public long UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
    }
}
