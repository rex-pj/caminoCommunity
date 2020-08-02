namespace Camino.Framework.Models
{
    public class BaseViewModel
    {
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }
    }
}
