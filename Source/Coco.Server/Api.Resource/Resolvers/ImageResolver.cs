﻿using Coco.Framework.Resolvers;
using Api.Public.Resolvers.Contracts;
using HotChocolate.Resolvers;
using Api.Resource.Models;
using Coco.Business.ValidationStrategies;
using Coco.Framework.Models;
using Coco.Common.Models;

namespace Api.Public.Resolvers
{
    public class ImageResolver : BaseResolver, IImageResolver
    {
        private readonly ValidationStrategyContext _validationStrategyContext;
        public ImageResolver(ValidationStrategyContext validationStrategyContext)
        {
            _validationStrategyContext = validationStrategyContext;
        }

        public ICommonResult ValidateImageUrl(IResolverContext context)
        {
            var model = context.Argument<ImageValidationModel>("criterias");
            _validationStrategyContext.SetStrategy(new ImageUrlValidationStrategy());
            if(model == null || string.IsNullOrEmpty(model.Url))
            {
                return CommonResult.Failed(new CommonError());
            }

            bool canUpdate = _validationStrategyContext.Validate(model.Url);

            if (!canUpdate)
            {
                _validationStrategyContext.SetStrategy(new Base64ImageValidationStrategy());
                canUpdate = _validationStrategyContext.Validate(model.Url);
            }

            if (canUpdate)
            {
                return CommonResult.Success();
            }
            return CommonResult.Failed(new CommonError());
        }
    }
}
