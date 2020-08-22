namespace Camino.Framework.Models
{
    public class UpdatePerItemModel
    {
        public object Key { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public bool CanEdit { get; set; }
    }
}
