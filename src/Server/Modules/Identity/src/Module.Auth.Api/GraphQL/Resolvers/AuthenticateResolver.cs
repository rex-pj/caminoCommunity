using Camino.Infrastructure.GraphQL.Resolvers;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Module.Auth.Api.GraphQL.Resolvers.Contracts;
using Module.Auth.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Module.Auth.Api.GraphQL.Resolvers
{
    public class AuthenticateResolver : BaseResolver, IAuthenticateResolver
    {
        private readonly IUserManager<ApplicationUser> _userManager;

        public AuthenticateResolver(IUserManager<ApplicationUser> userManager)
            : base()
        {
            _userManager = userManager;
        }

        public async Task<UserInfoModel> GetLoggedUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            long currentUserId = GetCurrentUserId(claimsPrincipal);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var userIdentityId = await _userManager.EncryptUserIdAsync(currentUserId);
            return new UserInfoModel
            {
                Address = currentUser.Address,
                BirthDate = currentUser.BirthDate,
                CountryCode = currentUser.CountryCode,
                CountryId = currentUser.CountryId,
                CountryName = currentUser.CountryName,
                Email = currentUser.Email,
                CreatedDate = currentUser.CreatedDate,
                Description = currentUser.Description,
                DisplayName = currentUser.DisplayName,
                Firstname = currentUser.Firstname,
                GenderId = currentUser.GenderId,
                GenderLabel = currentUser.GenderLabel,
                Lastname = currentUser.Lastname,
                PhoneNumber = currentUser.PhoneNumber,
                StatusId = currentUser.StatusId,
                StatusLabel = currentUser.StatusLabel,
                UpdatedDate = currentUser.UpdatedDate,
                UserIdentityId = userIdentityId
            };
        }
    }
}
