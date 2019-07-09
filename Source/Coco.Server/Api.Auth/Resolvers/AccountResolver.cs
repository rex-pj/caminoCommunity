using Api.Identity.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Commons.Helpers;
using Coco.Api.Framework.Mapping;
using Coco.Api.Framework.Models;
using Coco.Api.Framework.Resolvers;
using Coco.Business.Contracts;
using Coco.Common.Const;
using Coco.Entities.Enums;
using GraphQL;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Identity.Resolvers
{
    public class AccountResolver : BaseResolver
    {
        private readonly IAccountManager<ApplicationUser> _accountManager;
        private readonly ICountryBusiness _countryBusiness;

        public AccountResolver(IAccountManager<ApplicationUser> accountManager,
            ICountryBusiness countryBusiness)
        {
            _accountManager = accountManager;
            _countryBusiness = countryBusiness;
        }

        public UserInfo GetLoggedUser(IWorkContext workContext)
        {
            try
            {
                return UserInfoMapping.ApplicationUserToUserInfo(workContext.CurrentUser,
                    workContext.CurrentUser.UserIdentityId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResult> UpdateUserInfoAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UserInfoUpdateModel>("user");

                var parameters = new ApplicationUser()
                {
                    BirthDate = model.BirthDate,
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    GenderId = (byte)model.GenderId,
                    UpdatedDate = DateTime.Now,
                    UserName = model.Email,
                    Description = model.Description,
                    Address = model.Address,
                    CountryId = model.CountryId,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _accountManager.UpdateInfoAsync(parameters);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> UpdateUserInfoItemAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<UpdatePerItemModel>("criterias");
                var userContext = context.UserContext as IWorkContext;

                if (!model.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                if (userContext == null || userContext.CurrentUser == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var result = await _accountManager.UpdateInfoItemAsync(model, userContext.CurrentUser.AuthenticationToken);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }
    }
}
