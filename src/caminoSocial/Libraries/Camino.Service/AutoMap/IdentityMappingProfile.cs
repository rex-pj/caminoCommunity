using AutoMapper;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.Identity;
using Camino.Service.Data.Request;

namespace Camino.Service.AutoMap
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<UserProjection, UserInfo>();

            CreateMap<UserProjection, User>();

            CreateMap<User, UserProjection>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.PhoneNumber, opt => opt.MapFrom(s => s.UserInfo.PhoneNumber));

            CreateMap<User, UserFullProjection>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.GenderLabel, opt => opt.MapFrom(s => s.UserInfo.Gender.Name))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.CountryCode, opt => opt.MapFrom(s => s.UserInfo.Country.Code))
                .ForMember(t => t.CountryName, opt => opt.MapFrom(s => s.UserInfo.Country.Name))
                .ForMember(t => t.Description, opt => opt.MapFrom(s => s.UserInfo.Description))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.StatusLabel, opt => opt.MapFrom(s => s.Status.Name));

            CreateMap<UserAttribute, UserAttributeProjection>();
            CreateMap<AuthorizationPolicy, AuthorizationPolicyProjection>();
            CreateMap<AuthorizationPolicyProjection, AuthorizationPolicy>();
            CreateMap<UserAuthorizationPolicy, UserAuthorizationPolicyProjection>();
            CreateMap<Role, RoleProjection>();
            CreateMap<RoleProjection, Role>();
            CreateMap<UserClaimProjection, UserClaim>();
            CreateMap<UserClaim, UserClaimProjection>();
            CreateMap<UserToken, UserTokenProjection>();
            CreateMap<UserTokenProjection, UserToken>();
            CreateMap<UserLogin, UserLoginRequest>();
            CreateMap<UserLoginRequest, UserLogin>();
            CreateMap<RoleClaimProjection, RoleClaim>();
            CreateMap<RoleClaim, RoleClaimProjection>();
        }
    }
}
