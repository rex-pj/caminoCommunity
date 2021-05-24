namespace Camino.Shared.Results.Navigations
{
    public class ShortcutResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int TypeId { get; set; }
        public int Order { get; set; }
    }
}
