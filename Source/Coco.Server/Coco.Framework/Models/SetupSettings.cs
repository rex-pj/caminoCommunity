namespace Coco.Framework.Models
{
    public class SetupSettings
    {
        public bool HasSetupDatabase { get; set; }
        public string SetupUrl { get; set; }
        public bool IsInitialized { get; set; }
        public string CreateIdentityPath { get; set; }
    }
}
