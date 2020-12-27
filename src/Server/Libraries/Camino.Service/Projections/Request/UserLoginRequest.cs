namespace Camino.Service.Projections.Request
{
    public class UserLoginRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
    }
}
