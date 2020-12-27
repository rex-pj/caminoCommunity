namespace Camino.Framework.Models
{
    public class UpdatePerItemModel
    {
        public string Key { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public bool CanEdit { get; set; }
    }
}
