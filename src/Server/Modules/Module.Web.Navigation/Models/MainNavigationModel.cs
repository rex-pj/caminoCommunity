using System.Collections.Generic;

namespace Module.Web.Navigation.Models
{
    public class MainNavigationModel
    {
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }
        public IEnumerable<TabNavigationModel> TabNavigations { get; set; }
    }
}
