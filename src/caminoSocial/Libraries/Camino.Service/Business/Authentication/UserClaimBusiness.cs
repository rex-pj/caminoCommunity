using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Projections.Identity;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Camino.Service.Business.Authentication.Contracts;
using Camino.IdentityDAL.Entities;

namespace Camino.Service.Business.Authentication
{
    public class UserClaimBusiness : IUserClaimBusiness
    {
        private readonly IRepository<UserClaim> _userClaimRepository;
        private readonly IMapper _mapper;

        public UserClaimBusiness(IRepository<UserClaim> userClaimRepository, IMapper mapper)
        {
            _userClaimRepository = userClaimRepository;
            _mapper = mapper;
        }

        public void Add(UserClaimProjection userClaim)
        {
            var claim = _mapper.Map<UserClaim>(userClaim);
            _userClaimRepository.Add(claim);
        }

        public async Task<IList<UserClaimProjection>> GetByUserIdAsync(long userId)
        {
            var userClaims = await _userClaimRepository.GetAsync(x => x.UserId == userId);
            return _mapper.Map<IList<UserClaimProjection>>(userClaims);
        }

        public async Task<IList<UserClaimProjection>> GetByClaimAsync(long userId, string claimValue, string claimType)
        {
            var userClaims = await _userClaimRepository
                .GetAsync(x => x.UserId == userId && x.ClaimValue == claimValue && x.ClaimType == claimType);
            return _mapper.Map<IList<UserClaimProjection>>(userClaims);
        }

        public void Remove(UserClaimProjection userClaim)
        {
            var claim = _mapper.Map<UserClaim>(userClaim);
            _userClaimRepository.Delete(claim);
        }

        public async Task ReplaceClaimAsync(long userId, ClaimProjection claim, ClaimProjection newClaim)
        {
            var matchedClaims = await GetByClaimAsync(userId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<UserProjection>> GetUsersForClaimAsync(ClaimProjection claim)
        {
            var existUserClaims = await _userClaimRepository.Get(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type)
                .Select(x => new UserProjection() { 
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
