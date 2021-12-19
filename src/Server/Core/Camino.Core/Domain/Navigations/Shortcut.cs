using System;

namespace Camino.Core.Domain.Navigations
{
    public class Shortcut
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int TypeId { get; set; }
        public int DisplayOrder { get; set; }
        public int StatusId { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
