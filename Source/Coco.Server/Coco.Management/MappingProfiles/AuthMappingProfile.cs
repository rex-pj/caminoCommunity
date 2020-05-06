using AutoMapper;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Coco.Management.Models;

namespace Coco.Management.MappingProfiles
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<RoleDto, RoleViewModel>();
            CreateMap<RoleViewModel, RoleDto>();
            CreateMap<UserFullDto, UserViewModel>();
        }
    }
}
