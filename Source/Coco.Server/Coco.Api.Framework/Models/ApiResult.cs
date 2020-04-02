using Coco.Api.Framework.Commons.Enums;
using Coco.Commons.Models;
using System.Collections.Generic;

namespace Coco.Api.Framework.Models
{
    public interface IApiResult<TResult>
    {
        AccessModeEnum AccessMode { get; set; }
        bool IsSucceed { get; set; }
        TResult Result { get; set; }
        List<CommonError> Errors { get; set; }
    }

    public interface IApiResult : IApiResult<object>
    {
    }

    public class ApiResult : ApiResult<object>, IApiResult
    {
        public ApiResult() : this(false)
        {

        }

        public ApiResult(bool isSucceed = false)
        {
            IsSucceed = isSucceed;
            Errors = new List<CommonError>();
        }
    }

    public class ApiResult<TResult> : IApiResult<TResult>
    {
        public AccessModeEnum AccessMode { get; set; }
        public bool IsSucceed { get; set; }
        public List<CommonError> Errors { get; set; }
        public TResult Result { get; set; }

        public static IApiResult Success()
        {
            return new ApiResult(true);
        }

        public static IApiResult Success(TResult result)
        {
            var updateResult = Success();
            updateResult.Result = result;
            return updateResult;
        }

        public static IApiResult Success(TResult result, bool canEdit)
        {
            var accessMode = canEdit ? AccessModeEnum.CanEdit : AccessModeEnum.ReadOnly;
            var updateResult = Success(result);
            updateResult.AccessMode = accessMode;
            return updateResult;
        }

        public static IApiResult Failed(CommonError[] errors)
        {
            var result = new ApiResult();
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static IApiResult Failed(CommonError error)
        {
            return Failed(new CommonError[] { error });
        }
    }
}
