using Coco.Api.Framework.AccountIdentity.Entities;
using System.Collections.Generic;

namespace Coco.Api.Framework.Models
{
    public class UpdatePerItemResultModel
    {
        public UpdatePerItemResultModel(bool isSuccess = false)
        {
            IsSuccess = isSuccess;
            Errors = new List<IdentityError>();
        }

        public UpdatePerItemModel Result { get; set; }
        public bool IsSuccess { get; set; }
        public List<IdentityError> Errors { get; private set; }

        public static UpdatePerItemResultModel Success(UpdatePerItemModel result)
        {
            var updateResult = new UpdatePerItemResultModel(true);
            updateResult.Result = result;

            return updateResult;
        }

        public static UpdatePerItemResultModel Failed(IdentityError[] errors)
        {
            var result = new UpdatePerItemResultModel(false);
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        public static UpdatePerItemResultModel Failed(IdentityError error)
        {
            var result = new UpdatePerItemResultModel(false);
            if (error != null)
            {
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
