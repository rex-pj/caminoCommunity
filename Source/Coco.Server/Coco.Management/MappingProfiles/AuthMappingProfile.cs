using AutoMapper;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using Coco.Framework.Models;
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
            CreateMap<UserDto, UserViewModel>();
            CreateMap<AuthorizationPolicyDto, AuthorizationPolicyViewModel>();
            CreateMap<AuthorizationPolicyViewModel, AuthorizationPolicyDto>();
            CreateMap<AuthorizationPolicyUsersDto, AuthorizationPolicyUsersViewModel>();
            CreateMap<AuthorizationPolicyRolesDto, AuthorizationPolicyRolesViewModel>();
            CreateMap<UserRoleAuthorizationPoliciesDto, ApplicationUserRoleAuthorizationPolicy>();
            CreateMap<RoleAuthorizationPoliciesDto, ApplicationRole>();
            CreateMap<AuthorizationPolicyDto, ApplicationAuthorizationPolicy>();
        }
    }
}
