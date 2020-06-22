using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation.UserBusiness
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

        public void Add(UserClaimDto userClaim)
        {
            var claim = _mapper.Map<UserClaim>(userClaim);
            _userClaimRepository.Add(claim);
        }

        public async Task<IList<UserClaimDto>> GetByUserIdAsync(long userId)
        {
            var userClaims = await _userClaimRepository.GetAsync(x => x.UserId == userId);
            return _mapper.Map<IList<UserClaimDto>>(userClaims);
        }

        public async Task<IList<UserClaimDto>> GetByClaimAsync(long userId, string claimValue, string claimType)
        {
            var userClaims = await _userClaimRepository
                .GetAsync(x => x.UserId == userId && x.ClaimValue == claimValue && x.ClaimType == claimType);
            return _mapper.Map<IList<UserClaimDto>>(userClaims);
        }

        public void Remove(UserClaimDto userClaim)
        {
            var claim = _mapper.Map<UserClaim>(userClaim);
            _userClaimRepository.Delete(claim);
        }

        public async Task ReplaceClaimAsync(long userId, ClaimDto claim, ClaimDto newClaim)
        {
            var matchedClaims = await GetByClaimAsync(userId, claim.Value, claim.Type);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }

        public async Task<IList<UserDto>> GetUsersForClaimAsync(ClaimDto claim)
        {
            var existUserClaims = await _userClaimRepository.Get(x => x.ClaimValue == claim.Value && x.ClaimType == claim.Type)
                .Select(x => new UserDto() { 
                    Id = x.UserId,
                    DisplayName = x.User.DisplayName,
                    Lastname = x.User.Lastname,
                    Firstname = x.User.Firstname,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    IsActived = x.User.IsActived,
                    IsEmailConfirmed = x.User.IsEmailConfirmed,
                    StatusId = x.User.StatusId
                }).ToListAsync();

            return existUserClaims;
        }
    }
}
