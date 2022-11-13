namespace Camino.Application.Contracts
{
    public class PartialUpdateItemRequest
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
