namespace Camino.Service.Data.Request
{
    public class UpdateItemRequest
    {
        public object Key { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
