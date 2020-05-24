namespace Coco.Entities.Dtos.Auth
{
    public class UserClaimDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
