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
using Microsoft.AspNetCore.Http;
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

        public async Task<UserInfo> GetLoggedUserAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var userContext = context.UserContext as HttpContext;
                var userHeaderParams = HttpHelper.GetAuthorizationHeaders(userContext);

                var result = await _accountManager.GetLoggingUser(userHeaderParams.UserIdHashed, userHeaderParams.AuthenticationToken);
                if (result == null)
                {
                    return new UserInfo();
                }

                return UserInfoMapping.ApplicationUserToUserInfo(result, userHeaderParams.UserIdHashed);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApiResult> GetFullUserInfoAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<FindUserModel>("criterias");

                var userContext = context.UserContext as HttpContext;
                var headerParams = HttpHelper.GetAuthorizationHeaders(userContext);
                var userHashId = model.UserId;

                if (string.IsNullOrEmpty(model.UserId))
                {
                    userHashId = headerParams.UserIdHashed;
                }

                var user = await _accountManager.GetFullByHashIdAsync(userHashId);

                var result = UserInfoMapping.ApplicationUserToFullUserInfo(user);
                result.UserHashedId = userHashId;
                if (!user.AuthenticatorToken.Equals(headerParams.AuthenticationToken))
                {
                    return ApiResult<UserInfoExt>.Success(result);
                }

                var genderOptions = EnumHelper.EnumToSelectList<GenderEnum>();

                result.GenderSelections = genderOptions;
                var countries = _countryBusiness.GetAll();
                if (countries != null && countries.Any())
                {
                    result.CountrySelections = countries.Select(x => new SelectOption()
                    {
                        Id = x.Id.ToString(),
                        Text = x.Name
                    });
                }

                return ApiResult<UserInfoExt>.Success(result, true);
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ex.ToString(), ex);
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
                var userContext = context.UserContext as HttpContext;
                var userHeaderParams = HttpHelper.GetAuthorizationHeaders(userContext);

                if (!model.CanEdit)
                {
                    throw new UnauthorizedAccessException();
                }

                var result = await _accountManager.UpdateInfoItemAsync(model, userHeaderParams.AuthenticationToken);
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
