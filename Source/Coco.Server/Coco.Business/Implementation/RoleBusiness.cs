using AutoMapper;
using Coco.Business.Contracts;
using Coco.Contract;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Dtos.Auth;
using Coco.IdentityDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coco.Business.Implementation
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public RoleBusiness(IMapper mapper, IdentityDbContext dbContext, IRepository<Role> roleRepository)
        {
            _dbContext = dbContext;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        #region CRUD
        public byte Add(RoleDto roleModel)
        {
            if (roleModel == null)
            {
                throw new ArgumentNullException(nameof(roleModel));
            }

            var role = _mapper.Map<Role>(roleModel);

            _roleRepository.Add(role);
            _dbContext.SaveChanges();
            return role.Id;
        }

        public bool Delete(byte id)
        {
            var role = _roleRepository.Find(id);
            _roleRepository.Delete(role);
            _dbContext.SaveChanges();

            return true;
        }

        public RoleDto Find(byte id)
        {
            var role = _roleRepository.Find(id);
            var roleModel = _mapper.Map<RoleDto>(role);
            return roleModel;
        }

        public async Task<List<RoleDto>> GetAsync()
        {
            var roles = await _roleRepository.GetAsync();
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            return roleDtos;
        }

        public bool Update(RoleDto roleModel)
        {
            if (roleModel.Id <= 0)
            {
                throw new ArgumentNullException("Role Id");
            }

            var role = _roleRepository.Find(roleModel.Id);
            role.Description = roleModel.Description;
            role.Name = role.Name;

            _roleRepository.Update(role);
            _dbContext.SaveChanges();

            return true;
        }

        public RoleDto GetByName(string name)
        {
            var role = _roleRepository.Get(x => x.Name.Equals(name))
                .FirstOrDefault();

            var roleModel = _mapper.Map<RoleDto>(role);

            return roleModel;
        }
        #endregion

        #region Privates
        private Role RoleModelToEntity(RoleDto roleModel)
        {
            Role role = new Role() {
                Description = roleModel.Description,
                Name = roleModel.Name
            };

            return role;
        }

        private RoleDto RoleEntityToModel(Role role)
        {
            RoleDto roleModel = new RoleDto()
            {
                Description = role.Description,
                Name = role.Name,
                Id = role.Id
            };

            return roleModel;
        }
        #endregion
    }
}
