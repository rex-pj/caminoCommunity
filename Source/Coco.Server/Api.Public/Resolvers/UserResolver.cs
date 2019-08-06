using Api.Public.Models;
using Coco.Api.Framework.UserIdentity.Contracts;
using Coco.Api.Framework.Commons.Encode;
using Coco.Api.Framework.Commons.Helpers;
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
using AutoMapper;

namespace Api.Public.Resolvers
{
    public class UserResolver : BaseResolver
    {
        private readonly ILoginManager<ApplicationUser> _loginManager;
        private readonly IUserManager<ApplicationUser> _userManager;
        private readonly ICountryBusiness _countryBusiness;
        private readonly IMapper _mapper;

        public UserResolver(ILoginManager<ApplicationUser> loginManager,
            IUserManager<ApplicationUser> userManager,
            IMapper mapper,
            ICountryBusiness countryBusiness)
        {
            _loginManager = loginManager;
            _userManager = userManager;
            _countryBusiness = countryBusiness;
            _mapper = mapper;
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

                var result = await _userManager.CreateAsync(parameters);
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

                var userContext = context.UserContext as ISessionContext;
                if (string.IsNullOrEmpty(model.UserId) && userContext != null
                    && userContext.CurrentUser != null)
                {
                    userIdentityId = userContext.CurrentUser.UserIdentityId;
                }

                var user = await _userManager.GetFullByHashIdAsync(userIdentityId);

                var result = _mapper.Map<UserInfoExt>(user);
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
