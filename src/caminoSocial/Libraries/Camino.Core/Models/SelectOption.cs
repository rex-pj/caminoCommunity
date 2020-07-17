namespace Camino.Core.Models
{
    public interface ISelectOption
    {
        string Id { get; set; }
        string Text { get; set; }
        bool IsSelected { get; set; }
    }

    public class SelectOption : ISelectOption
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }
}
