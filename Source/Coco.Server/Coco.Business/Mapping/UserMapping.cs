using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.Account;
using System;
using System.Linq.Expressions;

namespace Coco.Business.Mapping
{
    public static class UserMapping
    {
        public static UserInfo UserModelToEntity(UserModel userModel)
        {
            UserInfo user = new UserInfo
            {
                GenderId = userModel.GenderId,
                Address = userModel.Address,
                BirthDate = userModel.BirthDate,
                CountryId = userModel.CountryId,
                Description = userModel.Description,
                PhoneNumber = userModel.PhoneNumber,
                User = new User()
                {
                    DisplayName = userModel.DisplayName,
                    Firstname = userModel.Firstname,
                    Lastname = userModel.Lastname,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    UpdatedById = userModel.UpdatedById,
                    CreatedById = userModel.CreatedById,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    PasswordSalt = userModel.PasswordSalt,
                    AuthenticatorToken = userModel.AuthenticationToken,
                    SecurityStamp = userModel.SecurityStamp,
                    Expiration = userModel.Expiration,
                    IsActived = userModel.IsActived,
                    StatusId = userModel.StatusId,
                }
            };

            return user;
        }

        public static Expression<Func<User, UserModel>> SelectorUserModel = user => new UserFullModel
        {
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            UpdatedDate = user.UpdatedDate,
            CreatedDate = user.CreatedDate,
            UpdatedById = user.UpdatedById,
            CreatedById = user.CreatedById,
            IsActived = user.IsActived,
            StatusId = user.StatusId,
            Email = user.Email,
            Password = user.Password,
            PasswordSalt = user.PasswordSalt,
            Expiration = user.Expiration,
            AuthenticationToken = user.AuthenticatorToken,
            SecurityStamp = user.SecurityStamp,
            Id = user.Id,

            GenderId = user.UserInfo.GenderId,
            Address = user.UserInfo.Address,
            BirthDate = user.UserInfo.BirthDate,
            CountryId = user.UserInfo.CountryId,
            PhoneNumber = user.UserInfo.PhoneNumber,
            AvatarUrl = user.UserInfo.AvatarUrl,
        };

        public static Expression<Func<User, UserFullModel>> SelectorFullUserModel = user => new UserFullModel
        {
            CreatedDate = user.CreatedDate,
            DisplayName = user.DisplayName,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            PhoneNumber = user.UserInfo.PhoneNumber,
            Description = user.UserInfo.Description,
            Address = user.UserInfo.Address,
            AvatarUrl = user.UserInfo.AvatarUrl,
            CoverPhotoUrl = user.UserInfo.CoverPhotoUrl,
            BirthDate = user.UserInfo.BirthDate,
            GenderId = user.UserInfo.GenderId,
            GenderLabel = user.UserInfo.Gender.Name,
            StatusId = user.StatusId,
            IsActived = user.IsActived,
            StatusLabel = user.Status.Name,

            CountryId = user.UserInfo.CountryId,
            CountryCode = user.UserInfo.Country.Code,
            CountryName = user.UserInfo.Country.Name,
            AuthenticationToken = user.AuthenticatorToken,
        };
    }
}
