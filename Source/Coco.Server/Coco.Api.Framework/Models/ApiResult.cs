using Coco.Api.Framework.Commons.Enums;
using System.Collections.Generic;

namespace Coco.Api.Framework.Models
{
    public class ApiResult<TResult> : ApiResult
    {
        public ApiResult(bool isSuccess = false) : base(isSuccess) { }

        public TResult Result { get; set; }

        public static ApiResult Success(TResult result)
        {
            var updateResult = new ApiResult<TResult>(true);
            updateResult.Result = result;
            return updateResult;
        }

        public static ApiResult Success(TResult result, bool canEdit)
        {
            var accessMode = canEdit ? AccessModifierEnum.CanEdit : AccessModifierEnum.ReadOnly;
            var updateResult = new ApiResult<TResult>(true)
            {
                Result = result,
                AccessMode = accessMode
            };
            return updateResult;
        }
    }

    public class ApiResult
    {
        public ApiResult(bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Errors = new List<ApiError>();
        }

        public AccessModifierEnum AccessMode { get; set; }

        public bool IsSuccess { get; set; }
        public List<ApiError> Errors { get; protected set; }

        public static ApiResult Failed(ApiError[] errors)
        {
            var result = new ApiResult(false);
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static ApiResult Failed(ApiError error)
        {
            var result = new ApiResult(false);
            if (error != null)
            {
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
