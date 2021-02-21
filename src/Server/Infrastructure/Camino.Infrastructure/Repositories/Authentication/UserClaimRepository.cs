using Camino.Core.Contracts.Data;
using Camino.Shared.Results.Identifiers;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Core.Contracts.Repositories.Authentication;
using Camino.Core.Domain.Identifiers;
using Camino.Shared.Results.Authorization;
using Camino.Shared.Requests.Authorization;

namespace Camino.Service.Repository.Authentication
{
    public class UserClaimRepository : IUserClaimRepository
    {
        private readonly IRepository<UserClaim> _userClaimRepository;

        public UserClaimRepository(IRepository<UserClaim> userClaimRepository)
        {
            _userClaimRepository = userClaimRepository;
        }

        public void Create(UserClaimRequest userClaim)
        {
            var claim = new UserClaim
            {
                ClaimType = userClaim.ClaimType,
                ClaimValue = userClaim.ClaimValue,
                UserId = userClaim.UserId
            };
            _userClaimRepository.Add(claim);
        }

        public async Task<IList<UserClaimResult>> GetByUserIdAsync(long userId)
        {
            var userClaims = await _userClaimRepository.Get(x => x.UserId == userId)
                .Select(x => new UserClaimResult
                {
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                    UserId = x.UserId,
                    Id = x.Id
                }).ToListAsync();
            return userClaims;
        }

        public async Task<IList<UserClaimResult>> GetByClaimAsync(long userId, string claimValue, string claimType)
        {
            var userClaims = await _userClaimRepository
                .Get(x => x.UserId == userId && x.ClaimValue == claimValue && x.ClaimType == claimType).Select(x => new UserClaimResult
                {
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                    UserId = x.UserId,
                    Id = x.Id
                }).ToListAsync();
            return userClaims;
        }

        public void Remove(UserClaimRequest userClaim)
        {
            var claim = new UserClaim
            {
                ClaimType = userClaim.ClaimType,
                ClaimValue = userClaim.ClaimValue,
                UserId = userClaim.UserId,
                Id = userClaim.Id
            };
            _userClaimRepository.Delete(claim);
        }

        public async Task ReplaceClaimAsync(long userId, ClaimRequest claim, ClaimRequest newClaim)
        {
            var matchedClaims = await GetByClaimAsync(userId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<UserResult>> GetUsersByClaimAsync(ClaimRequest claim)
        {
            var existUserClaims = await _userClaimRepository.Get(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type)
                .Select(x => new UserResult()
                {
                    Id = x.UserId,
                    DisplayName = x.User.DisplayName,
                    Lastname = x.User.Lastname,
                    Firstname = x.User.Firstname,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    IsEmailConfirmed = x.User.IsEmailConfirmed,
                    StatusId = x.User.StatusId
                }).ToListAsync();

            return existUserClaims;
        }
    }
}
