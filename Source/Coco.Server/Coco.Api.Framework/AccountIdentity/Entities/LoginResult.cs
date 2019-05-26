using System;
using System.Collections.Generic;

namespace Coco.Api.Framework.AccountIdentity.Entities
{
    public class LoginResult
    {
        public string AuthenticatorToken { get; set; }
        public DateTime? Expiration { get; set; }
        public bool IsSuccess { get; set; }
        public List<IdentityError> Errors { get; set; }

        public LoginResult(bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            AuthenticatorToken = string.Empty;
        }

        public static LoginResult Failed(IdentityError[] errors)
        {
            var result = new LoginResult(false);
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static LoginResult Failed(IdentityError error)
        {
            var result = new LoginResult(false);
            if (error != null)
            {
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
