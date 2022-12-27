using System.Reflection;

namespace Camino.Infrastructure.Modularity
{
    public class ModuleInfo
    {
        public string Name { get; set; }

        public Assembly Assembly { get; set; }

        public string ShortName
        {
            get
            {
                var nameSplitted = Name.Split('.');
                var middle = "";

                if (nameSplitted.Length >= 3)
                {
                    middle = nameSplitted[nameSplitted.Length - 2];
                }

                var last = nameSplitted.Last();
                if (string.IsNullOrEmpty(middle))
                {
                    return last.ToLower();
                }

                return $"{middle}-{last}".ToLower();
            }
        }

        public string Path { get; set; }
    }
}
