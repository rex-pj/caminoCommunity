namespace Camino.Service.Projections.Request
{
    public class UpdateItemRequest
    {
        public object Key { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
