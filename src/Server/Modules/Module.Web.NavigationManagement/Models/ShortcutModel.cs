using Camino.Framework.Models;
using Camino.Infrastructure.Commons.Enums;
using Camino.Shared.Enums;
using System;

namespace Module.Web.NavigationManagement.Models
{
    public class ShortcutModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public ShortcutType TypeId { get; set; }
        public int DisplayOrder { get; set; }
        public ShortcutStatus StatusId { get; set; }
        public long CreatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
