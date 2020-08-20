using System;

namespace Camino.Service.Data.Identity
{
    public class UserAttributeResult
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public bool IsDisabled { get; set; }
    }
}
