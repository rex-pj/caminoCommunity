using AutoMapper;
using Camino.Service.Data.Filters;
using Camino.Service.Data.Identity;
using Camino.IdentityManager.Models;
using Module.Web.AuthorizationManagement.Models;

namespace Module.Web.AuthorizationManagement.Infrastructure.AutoMap
{
    public class AuthorizationMappingProfile : Profile
    {
        public AuthorizationMappingProfile()
        {
            CreateMap<RoleResult, RoleModel>();
            CreateMap<RoleModel, RoleResult>();
            CreateMap<AuthorizationPolicyResult, AuthorizationPolicyModel>();
            CreateMap<AuthorizationPolicyModel, AuthorizationPolicyResult>();
            CreateMap<AuthorizationPolicyUsersResult, AuthorizationPolicyUsersModel>();
            CreateMap<AuthorizationPolicyRolesResult, AuthorizationPolicyRolesModel>();

            CreateMap<UserRoleAuthorizationPoliciesResult, ApplicationUserRoleAuthorizationPolicy>();
            CreateMap<RoleAuthorizationPoliciesResult, ApplicationRole>();
            CreateMap<AuthorizationPolicyResult, ApplicationAuthorizationPolicy>();
            CreateMap<ApplicationRole, RoleModel>();
            CreateMap<UserResult, UserModel>();
            CreateMap<UserModel, UserResult>();

            CreateMap<AuthorizationPolicyFilterModel, AuthorizationPolicyFilter>();
            CreateMap<RoleAuthorizationPolicyFilterModel, RoleAuthorizationPolicyFilter>();
            CreateMap<RoleFilterModel, RoleFilter>();

            CreateMap<UserAuthorizationPolicyFilterModel, UserAuthorizationPolicyFilter>();
        }
    }
}
