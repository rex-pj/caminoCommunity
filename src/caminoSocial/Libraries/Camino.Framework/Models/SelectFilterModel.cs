namespace Camino.Framework.Models
{
    public class SelectFilterModel
    {
        public string Query { get; set; }
        public long? CurrentId { get; set; }
        public bool IsParentOnly { get; set; }
    }
}
