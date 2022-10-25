using Camino.Shared.Enums;

namespace Module.Navigation.Api.Models
{
    public class ShortcutModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public ShortcutTypes TypeId { get; set; }
        public int Order { get; set; }
    }
}
