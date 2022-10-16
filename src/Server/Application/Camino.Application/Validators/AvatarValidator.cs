using Camino.Application.Contracts.AppServices.Users.Dtos;
using Camino.Core.Validators;
using Camino.Shared.Exceptions;
using Camino.Shared.Utils;

namespace Camino.Application.Validators
{
    public class AvatarValidator : BaseValidator<UserPhotoUpdateRequest, bool>
    {
        public override bool IsValid(UserPhotoUpdateRequest value)
        {
            if (value.FileData == null || value.FileData.Length == 0)
            {
                Errors = GetErrors(new ArgumentNullException(nameof(value.FileData))).ToList();
            }

            var image = ImageUtils.FileDataToImage(value.FileData);
            if (image.Width < 100 || image.Height < 100)
            {
                Errors = GetErrors(new PhotoSizeInvalidException(100, 100)).ToList();
            }

            return Errors == null || !Errors.Any();
        }

        public override IEnumerable<ValidatorErrorResult> GetErrors(Exception e)
        {
            yield return new ValidatorErrorResult
            {
                Message = e.Message
            };
        }
    }
}
