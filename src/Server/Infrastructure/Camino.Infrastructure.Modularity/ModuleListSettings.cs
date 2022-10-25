namespace Camino.Infrastructure.Modularity
{
    public class ModuleListSettings
    {
        public string Path { get; set; }
        public IList<ModuleSettings> Modules { get; set; }
    }
}
