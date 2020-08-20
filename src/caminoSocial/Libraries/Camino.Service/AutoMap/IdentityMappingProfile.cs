using AutoMapper;
using Camino.IdentityDAL.Entities;
using Camino.Service.Data.Identity;

namespace Camino.Service.AutoMap
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<UserResult, UserInfo>();

            CreateMap<UserResult, User>();

            CreateMap<User, UserResult>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.PhoneNumber, opt => opt.MapFrom(s => s.UserInfo.PhoneNumber));

            CreateMap<User, UserFullDto>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.GenderLabel, opt => opt.MapFrom(s => s.UserInfo.Gender.Name))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.CountryCode, opt => opt.MapFrom(s => s.UserInfo.Country.Code))
                .ForMember(t => t.CountryName, opt => opt.MapFrom(s => s.UserInfo.Country.Name))
                .ForMember(t => t.Description, opt => opt.MapFrom(s => s.UserInfo.Description))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.StatusLabel, opt => opt.MapFrom(s => s.Status.Name));

            CreateMap<UserAttribute, UserAttributeResult>();
            CreateMap<AuthorizationPolicy, AuthorizationPolicyResult>();
            CreateMap<AuthorizationPolicyResult, AuthorizationPolicy>();
            CreateMap<UserAuthorizationPolicy, UserAuthorizationPolicyResult>();
            CreateMap<Role, RoleResult>();
            CreateMap<RoleResult, Role>();
            CreateMap<UserClaimResult, UserClaim>();
            CreateMap<UserClaim, UserClaimResult>();
            CreateMap<UserToken, UserTokenResult>();
            CreateMap<UserTokenResult, UserToken>();
            CreateMap<UserLogin, UserLoginDto>();
            CreateMap<UserLoginDto, UserLogin>();
            CreateMap<RoleClaimDto, RoleClaim>();
            CreateMap<RoleClaim, RoleClaimDto>();
        }
    }
}
