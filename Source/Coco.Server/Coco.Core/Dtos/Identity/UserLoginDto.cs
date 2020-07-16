namespace Coco.Core.Dtos.Identity
{
    public class UserLoginDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderKey { get; set; }
    }
}
