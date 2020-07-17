using AutoMapper;
using Camino.Business.Dtos.Identity;
using Camino.Framework.Models;
using Camino.Management.Models;

namespace Camino.Management.Infrastructure.AutoMap
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
            CreateMap<ApplicationRole, RoleViewModel>();
        }
    }
}
