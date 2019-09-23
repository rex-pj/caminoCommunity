using Coco.Entities.Base;

namespace Coco.Entities.Domain.Identity
{
    public class UserAttribute : BaseEntity
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
