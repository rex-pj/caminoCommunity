using System;

namespace Camino.Shared.Requests.Identifiers
{
    public class UserAttributeModifyRequest
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
