using AutoMapper;
using Camino.Business.Dtos.General;
using Camino.Business.Dtos.Identity;
using Camino.IdentityManager.Models;
using Module.Web.AuthorizationManagement.Models;

namespace Module.Web.AuthorizationManagement.Infrastructure.AutoMap
{
    public class AuthorizationMappingProfile : Profile
    {
        public AuthorizationMappingProfile()
        {
            CreateMap<RoleDto, RoleModel>();
            CreateMap<RoleModel, RoleDto>();
            CreateMap<AuthorizationPolicyDto, AuthorizationPolicyModel>();
            CreateMap<AuthorizationPolicyModel, AuthorizationPolicyDto>();
            CreateMap<AuthorizationPolicyUsersDto, AuthorizationPolicyUsersModel>();
            CreateMap<AuthorizationPolicyRolesDto, AuthorizationPolicyRolesModel>();

            CreateMap<UserRoleAuthorizationPoliciesDto, ApplicationUserRoleAuthorizationPolicy>();
            CreateMap<RoleAuthorizationPoliciesDto, ApplicationRole>();
            CreateMap<AuthorizationPolicyDto, ApplicationAuthorizationPolicy>();
            CreateMap<ApplicationRole, RoleModel>();
            CreateMap<UserDto, UserModel>();
            CreateMap<UserModel, UserDto>();

            CreateMap<AuthorizationPolicyFilterModel, AuthorizationPolicyFilterDto>();
            CreateMap<RoleAuthorizationPolicyFilterModel, RoleAuthorizationPolicyFilterDto>();
            CreateMap<RoleFilterModel, RoleFilterDto>();

            CreateMap<UserAuthorizationPolicyFilterModel, UserAuthorizationPolicyFilterDto>();
        }
    }
}
