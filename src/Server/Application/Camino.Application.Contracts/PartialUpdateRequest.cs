namespace Camino.Application.Contracts
{
    public class PartialUpdateRequest
    {
        public object Key { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
