namespace Coco.Entities.Domain.Identity
{
    public class UserToken
    {
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public long UserId { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
    }
}
