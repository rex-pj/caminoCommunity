using AutoMapper;
using Camino.Business.Contracts;
using Camino.Data.Contracts;
using Camino.Business.Dtos.Identity;
using Camino.Data.Entities.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Business.Dtos.General;

namespace Camino.Business.Implementation
{
    public class AuthorizationPolicyBusiness : IAuthorizationPolicyBusiness
    {
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public AuthorizationPolicyBusiness(IRepository<AuthorizationPolicy> authorizationPolicyRepository, IMapper mapper,
            IRepository<User> userRepository)
        {
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public AuthorizationPolicyDto Find(short id)
        {
            var authorizationPolicy = (from policy in _authorizationPolicyRepository.Table
                                       join createdBy in _userRepository.Table
                                       on policy.CreatedById equals createdBy.Id
                                       join updatedBy in _userRepository.Table
                                       on policy.UpdatedById equals updatedBy.Id
                                       where policy.Id == id
                                       select new AuthorizationPolicyDto()
                                       {
                                           CreatedById = policy.CreatedById,
                                           CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                                           CreatedDate = policy.CreatedDate,
                                           UpdatedById = policy.UpdatedById,
                                           UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                                           UpdatedDate = policy.UpdatedDate,
                                           Description = policy.Description,
                                           Id = policy.Id,
                                           Name = policy.Name
                                       }).FirstOrDefault();

            if (authorizationPolicy == null)
            {
                return null;
            }

            return authorizationPolicy;
        }

        public async Task<AuthorizationPolicyDto> FindByNameAsync(string name)
        {
            var exist = await _authorizationPolicyRepository.Get(x => x.Name == name).FirstOrDefaultAsync();
            if (exist == null)
            {
                return null;
            }

            var policy = _mapper.Map<AuthorizationPolicyDto>(exist);

            return policy;
        }

        public PageListDto<AuthorizationPolicyDto> Get(int page, int pageSize)
        {
            var query = (from policy in _authorizationPolicyRepository.Table
                         join createdBy in _userRepository.Table
                         on policy.CreatedById equals createdBy.Id
                         join updatedBy in _userRepository.Table
                         on policy.UpdatedById equals updatedBy.Id
                         select new AuthorizationPolicyDto()
                         {
                             CreatedById = policy.CreatedById,
                             CreatedByName = createdBy.Lastname + " " + createdBy.Firstname,
                             CreatedDate = policy.CreatedDate,
                             UpdatedById = policy.UpdatedById,
                             UpdatedByName = updatedBy.Lastname + " " + updatedBy.Firstname,
                             UpdatedDate = policy.UpdatedDate,
                             Description = policy.Description,
                             Id = policy.Id,
                             Name = policy.Name
                         });

            var filteredNumber = query.Select(x => x.Id).Count();
            
            var authorizationPolicies = query.Skip(pageSize * (page - 1))
                                         .Take(pageSize)
                                         .ToList();

            var result = new PageListDto<AuthorizationPolicyDto>(authorizationPolicies);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / pageSize);
            return result;
        }

        public long Add(AuthorizationPolicyDto authorizationPolicy)
        {
            var newPolicy = _mapper.Map<AuthorizationPolicy>(authorizationPolicy);
            newPolicy.UpdatedDate = DateTime.UtcNow;
            newPolicy.CreatedDate = DateTime.UtcNow;

            var id = _authorizationPolicyRepository.AddWithInt64Entity(newPolicy);
            return id;
        }

        public AuthorizationPolicyDto Update(AuthorizationPolicyDto policy)
        {
            var exist = _authorizationPolicyRepository.FirstOrDefault(x => x.Id == policy.Id);
            exist.Description = policy.Description;
            exist.Name = policy.Name;
            exist.UpdatedById = policy.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;

            _authorizationPolicyRepository.Update(exist);

            policy.UpdatedDate = exist.UpdatedDate;
            return policy;
        }

    }
}
