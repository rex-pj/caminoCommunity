using AutoMapper;
using Camino.Business.Dtos.Identity;
using Camino.Framework.Models;
using Module.Web.AuthorizationManagement.Models;

namespace Module.Web.AuthorizationManagement.Infrastructure.AutoMap
{
    public class AuthorizationMappingProfile : Profile
    {
        public AuthorizationMappingProfile()
        {
            CreateMap<RoleDto, RoleViewModel>();
            CreateMap<RoleViewModel, RoleDto>();
            CreateMap<AuthorizationPolicyDto, AuthorizationPolicyViewModel>();
            CreateMap<AuthorizationPolicyViewModel, AuthorizationPolicyDto>();
            CreateMap<AuthorizationPolicyUsersDto, AuthorizationPolicyUsersViewModel>();
            CreateMap<AuthorizationPolicyRolesDto, AuthorizationPolicyRolesViewModel>();

            CreateMap<UserRoleAuthorizationPoliciesDto, ApplicationUserRoleAuthorizationPolicy>();
            CreateMap<RoleAuthorizationPoliciesDto, ApplicationRole>();
            CreateMap<AuthorizationPolicyDto, ApplicationAuthorizationPolicy>();
            CreateMap<ApplicationRole, RoleViewModel>();
            CreateMap<UserDto, UserViewModel>();
            CreateMap<UserViewModel, UserDto>();
        }
    }
}
