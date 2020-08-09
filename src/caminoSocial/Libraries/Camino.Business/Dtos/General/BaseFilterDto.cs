namespace Camino.Business.Dtos.General
{
    public class BaseFilterDto
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Search { get; set; }
    }
}
