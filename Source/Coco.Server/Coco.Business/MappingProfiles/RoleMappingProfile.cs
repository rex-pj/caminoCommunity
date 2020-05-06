using AutoMapper;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Dtos.Auth;

namespace Coco.Business.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
        }
    }
}
