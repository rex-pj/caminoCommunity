using System.Collections.Generic;

namespace Module.Web.NavigationManagement.Models
{
    public class NavigationModel
    {
        public NavigationModel()
        {
            SubNavigations = new List<NavigationModel>();
            SubRoutes = new List<string>();
        }

        public string Icon { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public string Url { get; set; }
        public bool IsActived { get; set; }
        public IEnumerable<string> SubRoutes { get; set; }
        public IEnumerable<NavigationModel> SubNavigations { get; set; }
    }
}
