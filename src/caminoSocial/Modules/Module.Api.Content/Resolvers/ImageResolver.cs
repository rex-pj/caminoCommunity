using Camino.Framework.Resolvers;
using Module.Api.Content.Resolvers.Contracts;
using Module.Api.Content.Models;
using Camino.Business.ValidationStrategies;
using Camino.Framework.Models;
using Camino.Framework.SessionManager.Core;
using Camino.Business.Dtos.General;
using Camino.Core.Models;

namespace Module.Api.Content.Resolvers
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
