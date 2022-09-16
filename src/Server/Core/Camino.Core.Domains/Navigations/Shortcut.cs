using System;

namespace Camino.Core.Domains.Navigations
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
        public DateTime CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
