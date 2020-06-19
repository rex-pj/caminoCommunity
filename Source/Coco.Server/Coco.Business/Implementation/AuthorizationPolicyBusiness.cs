using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos.Auth;
using Coco.IdentityDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coco.Business.Implementation
{
    public class AuthorizationPolicyBusiness : IAuthorizationPolicyBusiness
    {
        private readonly IRepository<AuthorizationPolicy> _authorizationPolicyRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IdentityDbConnection _identityDbContext;
        public AuthorizationPolicyBusiness(IRepository<AuthorizationPolicy> authorizationPolicyRepository, IMapper mapper, IdentityDbConnection identityDbContext, 
            IRepository<User> userRepository)
        {
            _authorizationPolicyRepository = authorizationPolicyRepository;
            _mapper = mapper;
            _identityDbContext = identityDbContext;
            _userRepository = userRepository;
        }

        public AuthorizationPolicyDto Find(short id)
        {
            var exist = _authorizationPolicyRepository.FirstOrDefault(x => x.Id == id);
            if (exist == null)
            {
                return null;
            }

            var createdByUser = _userRepository.FirstOrDefault(x => x.Id == exist.CreatedById);
            var updatedByUser = _userRepository.FirstOrDefault(x => x.Id == exist.UpdatedById);

            var policy = _mapper.Map<AuthorizationPolicyDto>(exist);
            policy.CreatedByName = createdByUser.DisplayName;
            policy.UpdatedByName = updatedByUser.DisplayName;

            return policy;
        }

        public AuthorizationPolicyDto FindByName(string name)
        {
            var exist = _authorizationPolicyRepository.Get(x => x.Name == name).FirstOrDefault();
            if (exist == null)
            {
                return null;
            }

            var policy = _mapper.Map<AuthorizationPolicyDto>(exist);

            return policy;
        }

        public List<AuthorizationPolicyDto> GetFull()
        {
            var authorizationPolicies = _authorizationPolicyRepository.Get()
                .Select(a => new AuthorizationPolicyDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    CreatedById = a.CreatedById,
                    CreatedDate = a.CreatedDate,
                    Description = a.Description,
                    UpdatedById = a.UpdatedById,
                    UpdatedDate = a.UpdatedDate
                }).ToList();

            var createdByIds = authorizationPolicies.Select(x => x.CreatedById).ToArray();
            var updatedByIds = authorizationPolicies.Select(x => x.UpdatedById).ToArray();

            var createdByUsers = _userRepository.Get(x => createdByIds.Contains(x.Id)).ToList();
            var updatedByUsers = _userRepository.Get(x => updatedByIds.Contains(x.Id)).ToList();

            foreach (var authorizationPolicy in authorizationPolicies)
            {
                var createdBy = createdByUsers.FirstOrDefault(x => x.Id == authorizationPolicy.CreatedById);
                authorizationPolicy.CreatedByName = createdBy.DisplayName;

                var updatedBy = createdByUsers.FirstOrDefault(x => x.Id == authorizationPolicy.CreatedById);
                authorizationPolicy.UpdatedByName = updatedBy.DisplayName;
            }

            return authorizationPolicies;
        }

        public long Add(AuthorizationPolicyDto authorizationPolicy)
        {
            var newPolicy = _mapper.Map<AuthorizationPolicy>(authorizationPolicy);
            newPolicy.UpdatedDate = DateTime.UtcNow;
            newPolicy.CreatedDate = DateTime.UtcNow;

            _authorizationPolicyRepository.Add(newPolicy);
            return newPolicy.Id;
            //return _identityDbContext.SaveChanges();
        }

        public AuthorizationPolicyDto Update(AuthorizationPolicyDto policy)
        {
            var exist = _authorizationPolicyRepository.FirstOrDefault(x => x.Id == policy.Id);
            exist.Description = policy.Description;
            exist.Name = policy.Name;
            exist.UpdatedById = policy.UpdatedById;
            exist.UpdatedDate = DateTime.UtcNow;

            _authorizationPolicyRepository.Update(exist);
            //_identityDbContext.SaveChanges();

            policy.UpdatedDate = exist.UpdatedDate;
            return policy;
        }

    }
}
