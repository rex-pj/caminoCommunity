using Camino.Framework.GraphQL.Resolvers;
using Module.Api.Content.GraphQL.Resolvers.Contracts;
using Module.Api.Content.Models;
using Camino.Service.Strategies.Validation;
using Camino.Framework.Models;
using Camino.Core.Models;
using Camino.IdentityManager.Contracts.Core;

namespace Module.Api.Content.GraphQL.Resolvers
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
