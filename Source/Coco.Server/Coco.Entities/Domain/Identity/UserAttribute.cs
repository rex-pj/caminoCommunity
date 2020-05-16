using System;

namespace Coco.Entities.Domain.Identity
{
    public class UserAttribute
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public bool IsDisabled { get; set; }
    }
}
