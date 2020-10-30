using AutoMapper;
using Camino.Data.Contracts;
using Camino.Service.Projections.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Camino.Service.Projections.Filters;
using Camino.Service.Business.Authorization.Contracts;
using Camino.IdentityDAL.Entities;
using Camino.Service.Projections.PageList;

namespace Camino.Service.Business.Authorization
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

        public AuthorizationPolicyProjection Find(short id)
        {
            var authorizationPolicy = (from policy in _authorizationPolicyRepository.Table
                                       join createdBy in _userRepository.Table
                                       on policy.CreatedById equals createdBy.Id
                                       join updatedBy in _userRepository.Table
                                       on policy.UpdatedById equals updatedBy.Id
                                       where policy.Id == id
                                       select new AuthorizationPolicyProjection()
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

        public async Task<AuthorizationPolicyProjection> FindByNameAsync(string name)
        {
            var exist = await _authorizationPolicyRepository.Get(x => x.Name == name).FirstOrDefaultAsync();
            if (exist == null)
            {
                return null;
            }

            var policy = _mapper.Map<AuthorizationPolicyProjection>(exist);

            return policy;
        }

        public BasePageList<AuthorizationPolicyProjection> Get(AuthorizationPolicyFilter filter)
        {
            var search = filter.Search != null ? filter.Search.ToLower() : "";
            var query = (from policy in _authorizationPolicyRepository.Table
                         join createdBy in _userRepository.Table
                         on policy.CreatedById equals createdBy.Id
                         join updatedBy in _userRepository.Table
                         on policy.UpdatedById equals updatedBy.Id
                         where string.IsNullOrEmpty(search) || policy.Name.ToLower().Contains(search)
                         || (policy.Description != null && policy.Description.ToLower().Contains(search))
                         select new AuthorizationPolicyProjection()
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

            var authorizationPolicies = query.Skip(filter.PageSize * (filter.Page - 1))
                                         .Take(filter.PageSize)
                                         .ToList();

            var result = new BasePageList<AuthorizationPolicyProjection>(authorizationPolicies);
            result.TotalResult = filteredNumber;
            result.TotalPage = (int)Math.Ceiling((double)filteredNumber / filter.PageSize);
            return result;
        }

        public long Create(AuthorizationPolicyProjection authorizationPolicy)
        {
            var newPolicy = _mapper.Map<AuthorizationPolicy>(authorizationPolicy);
            newPolicy.UpdatedDate = DateTime.UtcNow;
            newPolicy.CreatedDate = DateTime.UtcNow;

            var id = _authorizationPolicyRepository.AddWithInt64Entity(newPolicy);
            return id;
        }

        public AuthorizationPolicyProjection Update(AuthorizationPolicyProjection policy)
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
