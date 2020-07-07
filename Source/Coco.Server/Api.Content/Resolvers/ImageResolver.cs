using Coco.Framework.Resolvers;
using Api.Content.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Api.Content.Models;
using Coco.Business.ValidationStrategies;
using Coco.Framework.Models;
using Coco.Entities.Models;
using Coco.Framework.SessionManager.Core;

namespace Api.Content.Resolvers
{
    public class ImageResolver : BaseResolver, IImageResolver
    {
        private readonly ValidationStrategyContext _validationStrategyContext;
        public ImageResolver(ValidationStrategyContext validationStrategyContext, SessionState sessionState)
            : base(sessionState)
        {
            _validationStrategyContext = validationStrategyContext;
        }

        public ICommonResult ValidateImageUrl(ImageValidationModel criterias)
        {
            _validationStrategyContext.SetStrategy(new ImageUrlValidationStrategy());
            if(criterias == null || string.IsNullOrEmpty(criterias.Url))
            {
                return CommonResult.Failed(new CommonError());
            }

            bool canUpdate = _validationStrategyContext.Validate(criterias.Url);

            if (!canUpdate)
            {
                _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(criterias.Url);
            }

            if (canUpdate)
            {
                return CommonResult.Success();
            }
            return CommonResult.Failed(new CommonError());
        }
    }
}
