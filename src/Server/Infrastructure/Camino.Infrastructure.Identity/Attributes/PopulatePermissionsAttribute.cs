using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Infrastructure.Identity.Models;
using Camino.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Camino.Infrastructure.Identity.Attributes
{
    public class PopulatePermissionsAttribute : ResultFilterAttribute
    {
        private PolicyMethods[] _policyMethods;
        private readonly string _featureName;

        public PopulatePermissionsAttribute(string featureName, params PolicyMethods[] policyMethods)
        {
            _featureName = featureName;
            _policyMethods = policyMethods;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is null)
            {
                await next();
            }

            var viewResult = context.Result as ViewResult;
            var partialViewResult = context.Result as PartialViewResult;
            if (viewResult == null && partialViewResult == null)
            {
                await next();
            }

            var viewModel = viewResult == null ? viewResult?.Model : partialViewResult?.Model;
            if (viewModel is not BaseIdentityModel)
            {
                await next();
            }

            var httpContext = context.HttpContext;
            if (_featureName != null && !_policyMethods.Any())
            {
                _policyMethods = Enum.GetValues(typeof(PolicyMethods)).Cast<PolicyMethods>().ToArray();
            }

            var policies = _policyMethods.Select(x => $"{x}{_featureName}").ToArray();
            var requestServices = httpContext.RequestServices;
            var userManager = requestServices.GetRequiredService<IUserManager<ApplicationUser>>();
            var numberOfPolicies = policies.Length;
            var model = viewModel as BaseIdentityModel;
            for (int i = 0; i < numberOfPolicies; i++)
            {
                var policyMethod = _policyMethods[i].ToString();
                var propertyInfo = model?.GetType().GetProperty(policyMethod);

                var hasPolicy = await userManager.HasPolicyAsync(httpContext.User, policies[i]);
                propertyInfo?.SetValue(model, hasPolicy, null);
            }

            await next();
        }
    }
}
