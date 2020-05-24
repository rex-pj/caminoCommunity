using AutoMapper;
using Coco.Entities.Domain.Auth;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Dtos;
using Coco.Entities.Dtos.Auth;
using Coco.Entities.Dtos.User;
using System;

namespace Coco.Business.MappingProfiles
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<UserDto, UserInfo>()
                .ForMember(t => t.User, opt => opt.MapFrom(s => new User()
                {
                    DisplayName = s.DisplayName,
                    Firstname = s.Firstname,
                    Lastname = s.Lastname,
                    UserName = s.UserName,
                    UpdatedDate = s.CreatedDate,
                    CreatedDate = s.UpdatedDate,
                    UpdatedById = s.UpdatedById,
                    CreatedById = s.CreatedById,
                    Email = s.Email,
                    PasswordHash = s.PasswordHash,
                    IsActived = s.IsActived,
                    StatusId = s.StatusId
                }));

            CreateMap<User, UserDto>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.PhoneNumber, opt => opt.MapFrom(s => s.UserInfo.PhoneNumber))
                .ForMember(t => t.AvatarUrl, opt => opt.MapFrom(s => s.UserInfo.AvatarUrl))
                .ForMember(t => t.CoverPhotoUrl, opt => opt.MapFrom(s => s.UserInfo.CoverPhotoUrl));

            CreateMap<User, UserFullDto>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.GenderLabel, opt => opt.MapFrom(s => s.UserInfo.Gender.Name))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.CountryCode, opt => opt.MapFrom(s => s.UserInfo.Country.Code))
                .ForMember(t => t.CountryName, opt => opt.MapFrom(s => s.UserInfo.Country.Name))
                .ForMember(t => t.AvatarUrl, opt => opt.MapFrom(s => s.UserInfo.AvatarUrl))
                .ForMember(t => t.CoverPhotoUrl, opt => opt.MapFrom(s => s.UserInfo.CoverPhotoUrl))
                .ForMember(t => t.Description, opt => opt.MapFrom(s => s.UserInfo.Description))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.StatusLabel, opt => opt.MapFrom(s => s.Status.Name));

            CreateMap<UserPhoto, UserPhotoDto>();
            CreateMap<UserAttribute, UserAttributeDto>();
            CreateMap<AuthorizationPolicy, AuthorizationPolicyDto>();
            CreateMap<AuthorizationPolicyDto, AuthorizationPolicy>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<UserClaimDto, UserClaim>();
            CreateMap<UserClaim, UserClaimDto>();
            CreateMap<UserToken, UserTokenDto>();
            CreateMap<UserTokenDto, UserToken>();
        }
    }
}
