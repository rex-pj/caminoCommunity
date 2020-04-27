using Coco.Framework.Commons.Enums;
using Coco.Commons.Models;
using System.Collections.Generic;

namespace Coco.Framework.Models
{
    public interface ICommonResult<TResult>
    {
        AccessModeEnum AccessMode { get; set; }
        bool IsSucceed { get; set; }
        TResult Result { get; set; }
        List<CommonError> Errors { get; set; }
    }

    public interface ICommonResult : ICommonResult<object>
    {
    }

    public class CommonResult : CommonResult<object>, ICommonResult
    {
        public CommonResult() : this(false)
        {

        }

        public CommonResult(bool isSucceed = false)
        {
            IsSucceed = isSucceed;
            Errors = new List<CommonError>();
        }
    }

    public class CommonResult<TResult> : ICommonResult<TResult>
    {
        public AccessModeEnum AccessMode { get; set; }
        public bool IsSucceed { get; set; }
        public List<CommonError> Errors { get; set; }
        public TResult Result { get; set; }

        public static ICommonResult Success()
        {
            return new CommonResult(true);
        }

        public static ICommonResult Success(TResult result)
        {
            var updateResult = Success();
            updateResult.Result = result;
            return updateResult;
        }

        public static ICommonResult Success(TResult result, bool canEdit)
        {
            var accessMode = canEdit ? AccessModeEnum.CanEdit : AccessModeEnum.ReadOnly;
            var updateResult = Success(result);
            updateResult.AccessMode = accessMode;
            return updateResult;
        }

        public static ICommonResult Failed(CommonError[] errors)
        {
            var result = new CommonResult();
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static ICommonResult Failed(CommonError error)
        {
            return Failed(new CommonError[] { error });
        }
    }
}
