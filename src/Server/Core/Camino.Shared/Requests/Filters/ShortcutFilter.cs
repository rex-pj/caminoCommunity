namespace Camino.Shared.Requests.Filters
{
    public class ShortcutFilter : BaseFilter
    {
        public int? TypeId { get; set; }
        public bool CanGetInactived { get; set; }
        public int? StatusId { get; set; }
    }
}
