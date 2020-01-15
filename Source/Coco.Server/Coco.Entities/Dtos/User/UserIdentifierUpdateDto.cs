namespace Coco.Entities.Dtos.User
{
    public class UserIdentifierUpdateDto
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string UserIdentityId { get; set; }
        public string AuthenticationToken { get; set; }
        public long Id { get; set; }
    }
}
