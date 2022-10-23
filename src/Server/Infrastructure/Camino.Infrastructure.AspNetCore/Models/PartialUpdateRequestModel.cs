namespace Camino.Infrastructure.AspNetCore.Models
{
    public class PartialUpdateRequestModel
    {
        public string Key { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public bool CanEdit { get; set; }
    }
}
