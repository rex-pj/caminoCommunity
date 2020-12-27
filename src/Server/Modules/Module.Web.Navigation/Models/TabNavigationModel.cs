using System.Collections.Generic;

namespace Module.Web.Navigation.Models
{
    public class TabNavigationModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActived { get; set; }
        public IEnumerable<NavigationModel> Navigations { get; set; }
    }
}
