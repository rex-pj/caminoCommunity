namespace Camino.IdentityManager.Contracts.Core
{
    public class CrypterSettings
    {
        public const string Name = "Crypter";
        public string PepperKey { get; set; }
        public string SaltKey { get; set; }
        public string SecretKey { get; set; }
    }
}
