namespace Camino.Application.Contracts
{
    public class PartialUpdateRequest
    {
        public object Key { get; set; }
        public IList<PartialUpdateItemRequest> Updates { get; set; }
    }
}
