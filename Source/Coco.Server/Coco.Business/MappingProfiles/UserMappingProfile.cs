using AutoMapper;
using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.User;
using System;

namespace Coco.Business.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserModel, UserInfo>()
                .ForMember(t => t.User, opt => opt.MapFrom(s => new User()
                {
                    DisplayName = s.DisplayName,
                    Firstname = s.Firstname,
                    Lastname = s.Lastname,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    UpdatedById = s.UpdatedById,
                    CreatedById = s.CreatedById,
                    Email = s.Email,
                    Password = s.Password,
                    PasswordSalt = s.PasswordSalt,
                    AuthenticatorToken = s.AuthenticationToken,
                    SecurityStamp = s.SecurityStamp,
                    Expiration = s.Expiration,
                    IsActived = s.IsActived,
                    StatusId = s.StatusId,
                }));

            CreateMap<User, UserModel>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.Address, opt => opt.MapFrom(s => s.UserInfo.Address))
                .ForMember(t => t.BirthDate, opt => opt.MapFrom(s => s.UserInfo.BirthDate))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.PhoneNumber, opt => opt.MapFrom(s => s.UserInfo.PhoneNumber))
                .ForMember(t => t.AvatarUrl, opt => opt.MapFrom(s => s.UserInfo.AvatarUrl))
                .ForMember(t => t.CoverPhotoUrl, opt => opt.MapFrom(s => s.UserInfo.CoverPhotoUrl));

            CreateMap<User, UserLoggedInModel>()
                .ForMember(t => t.GenderId, opt => opt.MapFrom(s => s.UserInfo.GenderId))
                .ForMember(t => t.CountryId, opt => opt.MapFrom(s => s.UserInfo.CountryId))
                .ForMember(t => t.AvatarUrl, opt => opt.MapFrom(s => s.UserInfo.AvatarUrl))
                .ForMember(t => t.CoverPhotoUrl, opt => opt.MapFrom(s => s.UserInfo.CoverPhotoUrl));

            CreateMap<User, UserFullModel>()
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
        }
    }
}
