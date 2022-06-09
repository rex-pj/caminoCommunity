namespace Camino.Application.Contracts.AppServices.Navigations.Dtos
{
    public class ShortcutFilter : BaseFilter
    {
        public int? TypeId { get; set; }
        public bool CanGetInactived { get; set; }
        public int? StatusId { get; set; }
    }
}
