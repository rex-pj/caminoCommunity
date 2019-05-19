using System.Collections.Generic;

namespace Coco.Api.Framework.AccountIdentity.Entities
{
    public class IdentityResult
    {
        public IdentityResult(bool isSuccess = false) {
            IsSuccess = isSuccess;
            Errors = new List<IdentityError>();
        }

        public bool IsSuccess { get; set; }
        public List<IdentityError> Errors { get; private set; }

        public static IdentityResult Failed(IdentityError[] errors)
        {
            var result = new IdentityResult(false);
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static IdentityResult Failed(IdentityError error)
        {
            var result = new IdentityResult(false);
            if (error != null)
            {
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
