using Api.Public.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using Coco.Api.Framework.Commons.Encode;
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

namespace Api.Public.Resolvers
{
    public class AccountResolver : BaseResolver
    {
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IAccountManager<ApplicationUser> _accountManager;
        private readonly ICountryBusiness _countryBusiness;

        public AccountResolver(ILoginManager<ApplicationUser> loginManager,
            IAccountManager<ApplicationUser> accountManager,
            ICountryBusiness countryBusiness)
        {
            _loginManager = loginManager;
            _accountManager = accountManager;
            _countryBusiness = countryBusiness;
        }

        public async Task<ApiResult> SigninAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<SigninModel>("signinModel");

                var result = await _loginManager.LoginAsync(model.Username, model.Password);

                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> SignupAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<RegisterModel>("user");

                var parameters = new ApplicationUser()
                {
                    BirthDate = model.BirthDate,
                    CreatedDate = DateTime.Now,
                    DisplayName = $"{model.Lastname} {model.Firstname}",
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    GenderId = (byte)model.GenderId,
                    StatusId = (byte)UserStatusEnum.New,
                    UpdatedDate = DateTime.Now,
                    UserName = model.Email,
                    PasswordSalt = SaltGenerator.GetSalt(),
                    SecurityStamp = SaltGenerator.GetSalt(),
                    Password = model.Password
                };

                var result = await _accountManager.CreateAsync(parameters);
                HandleContextError(context, result.Errors);

                return result;
            }
            catch (Exception ex)
            {
                throw new ExecutionError(ErrorMessageConst.EXCEPTION, ex);
            }
        }

        public async Task<ApiResult> GetFullUserInfoAsync(ResolveFieldContext<object> context)
        {
            try
            {
                var model = context.GetArgument<FindUserModel>("criterias");
                
                var userIdentityId = model.UserId;

                var userContext = context.UserContext as IWorkContext;
                if (string.IsNullOrEmpty(model.UserId) && userContext != null
                    && userContext.CurrentUser != null)
                {
                    userIdentityId = userContext.CurrentUser.UserIdentityId;
                }

                var user = await _accountManager.GetFullByHashIdAsync(userIdentityId);

                var result = UserInfoMapping.FullUserModelToInfo(user);
                result.UserIdentityId = userIdentityId;
                if (userContext.CurrentUser == null || !user.AuthenticationToken.Equals(userContext.AuthenticationToken))
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
    }
}
