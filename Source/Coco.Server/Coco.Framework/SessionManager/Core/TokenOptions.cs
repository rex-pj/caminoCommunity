using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Framework.SessionManager.Core
{
    public class TokenOptions
    {
        public static readonly string DefaultProvider = "Default";
        public static readonly string DefaultEmailProvider = "Email";


        public string PasswordResetTokenProvider { get; set; } = DefaultProvider;
        public string EmailConfirmationTokenProvider { get; set; } = DefaultProvider;
    }
}
