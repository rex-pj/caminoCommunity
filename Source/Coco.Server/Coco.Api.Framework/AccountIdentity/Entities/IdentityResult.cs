using System.Collections.Generic;

namespace Coco.Api.Framework.AccountIdentity.Entities
{
    public class IdentityResult
    {
        public IdentityResult(bool isSuccess) {
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
    }
}
