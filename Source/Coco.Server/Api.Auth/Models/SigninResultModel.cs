using System;

namespace Api.Auth.Models
{
    public class SigninResultModel
    {
        public string AuthenticatorToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
