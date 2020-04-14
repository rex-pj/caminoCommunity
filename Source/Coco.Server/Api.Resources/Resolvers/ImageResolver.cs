using Coco.Api.Framework.Resolvers;
using Api.Public.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Api.Resources.Models;
using Coco.Business.ValidationStrategies;

namespace Api.Public.Resolvers
{
    public class ImageResolver : BaseResolver, IImageResolver
    {
        private readonly ValidationStrategyContext _validationStrategyContext;
        public ImageResolver(ValidationStrategyContext validationStrategyContext)
        {
            _validationStrategyContext = validationStrategyContext;
        }

        public ImageValidationModel ValidateImageUrl(IResolverContext context)
        {
            var model = context.Argument<ImageValidationModel>("criterias");
            _validationStrategyContext.SetStrategy(new ImageUrlValidationStrategy());
            bool canUpdate = _validationStrategyContext.Validate(model.Url);

            return new ImageValidationModel() { 
                IsValid = canUpdate
            };
        }
    }
}
