using Camino.Framework.Models;

namespace Module.Web.NavigationManagement.Models
{
    public class ShortcutModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int TypeId { get; set; }
    }
}
