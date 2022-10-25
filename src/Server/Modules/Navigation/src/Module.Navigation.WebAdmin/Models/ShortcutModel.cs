using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using System;

namespace Module.Navigation.WebAdmin.Models
{
    public class ShortcutModel : BaseIdentityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public ShortcutTypes TypeId { get; set; }
        public int DisplayOrder { get; set; }
        public ShortcutStatuses StatusId { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
