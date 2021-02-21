using Camino.Shared.Enums;
using System.Collections.Generic;
using Camino.Shared.General;

namespace Camino.Framework.Models
{
    public class CommonResult : CommonResult<object>
    {
        public CommonResult() : this(false)
        {

        }

        public CommonResult(bool isSucceed)
        {
            IsSucceed = isSucceed;
            Errors = new List<CommonError>();
        }
    }

    public class CommonResult<TResult>
    {
        public AccessMode AccessMode { get; set; }
        public bool IsSucceed { get; set; }
        public List<CommonError> Errors { get; set; }
        public TResult Result { get; set; }

        public static CommonResult Success()
        {
            return new CommonResult(true);
        }

        public static CommonResult Success(TResult result)
        {
            var updateResult = Success();
            updateResult.Result = result;
            return updateResult;
        }

        public static CommonResult Success(TResult result, bool canEdit)
        {
            var accessMode = canEdit ? AccessMode.CanEdit : AccessMode.ReadOnly;
            var updateResult = Success(result);
            updateResult.AccessMode = accessMode;
            return updateResult;
        }

        public static CommonResult Failed(IEnumerable<CommonError> errors)
        {
            var result = new CommonResult();
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static CommonResult Failed(CommonError error)
        {
            return Failed(new CommonError[] { error });
        }
    }
}
