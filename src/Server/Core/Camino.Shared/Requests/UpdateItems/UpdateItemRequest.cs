namespace Camino.Shared.Requests.UpdateItems
{
    public class UpdateItemRequest
    {
        public object Key { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
