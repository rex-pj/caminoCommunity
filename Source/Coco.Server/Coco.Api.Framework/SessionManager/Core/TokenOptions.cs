using System.Collections.Generic;

namespace Coco.Api.Framework.SessionManager.Core
{
    public class TokenOptions
    {
        public static readonly string DefaultProvider = "Default";
        public static readonly string DefaultEmailProvider = "Email";


        public string PasswordResetTokenProvider { get; set; } = DefaultProvider;
        public string EmailConfirmationTokenProvider { get; set; } = DefaultProvider;


        public Dictionary<string, TokenProviderDescriptor> ProviderMap { get; set; } = new Dictionary<string, TokenProviderDescriptor>();
    }
}
