using AutoMapper;
using Coco.Core.Dtos.Identity;
using Coco.Core.Entities.Identity;

namespace Coco.Business.AutoMap
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<UserDto, UserInfo>();

            CreateMap<UserDto, User>();

            CreateMap<User, UserDto>()
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

            CreateMap<UserAttribute, UserAttributeDto>();
            CreateMap<AuthorizationPolicy, AuthorizationPolicyDto>();
            CreateMap<AuthorizationPolicyDto, AuthorizationPolicy>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<UserClaimDto, UserClaim>();
            CreateMap<UserClaim, UserClaimDto>();
            CreateMap<UserToken, UserTokenDto>();
            CreateMap<UserTokenDto, UserToken>();
            CreateMap<UserLogin, UserLoginDto>();
            CreateMap<UserLoginDto, UserLogin>();
            CreateMap<RoleClaimDto, RoleClaim>();
            CreateMap<RoleClaim, RoleClaimDto>();
        }
    }
}
