using Coco.Entities.Domain.Identity;
using Coco.Entities.Model.Account;
using System;
using System.Web;

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

        public static UserModel UserEntityToModel(User user)
        {
            if (user == null)
            {
                return null;
            }

            UserModel userModel = new UserModel
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
                Id = user.Id
            };

            if (user.UserInfo != null)
            {
                userModel.GenderId = user.UserInfo.GenderId;
                userModel.Address = user.UserInfo.Address;
                userModel.BirthDate = user.UserInfo.BirthDate;
                userModel.CountryId = user.UserInfo.CountryId;
                userModel.PhoneNumber = user.UserInfo.PhoneNumber;
                userModel.Id = user.Id;
            }

            return userModel;
        }

        public static UserFullModel FullUserEntityToModel(User user)
        {
            if (user == null)
            {
                return null;
            }

            UserFullModel userModel = new UserFullModel
            {
                StatusId = user.StatusId,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate,
                UpdatedById = user.UpdatedById,
                CreatedById = user.CreatedById,
                IsActived = user.IsActived,
                Email = user.Email,
                Password = user.Password,
                PasswordSalt = user.PasswordSalt,
                Expiration = user.Expiration,
                AuthenticationToken = user.AuthenticatorToken,
                SecurityStamp = user.SecurityStamp,
                Id = user.Id,
                DisplayName = user.DisplayName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
            };

            if (user.Status != null)
            {
                userModel.StatusLabel = user.Status.Name;
            }

            if (user.UserInfo != null)
            {
                userModel.GenderId = user.UserInfo.GenderId;
                userModel.Address = user.UserInfo.Address;
                userModel.BirthDate = user.UserInfo.BirthDate;
                userModel.CountryId = user.UserInfo.CountryId;
                userModel.Description = user.UserInfo.Description;
                userModel.PhoneNumber = user.UserInfo.PhoneNumber;
                userModel.Photo = user.UserInfo.Photo;
                userModel.CoverPhoto = user.UserInfo.CoverPhoto;
            }

            if (user.UserInfo.Country != null)
            {
                userModel.CountryName = user.UserInfo.Country.Name;
                userModel.CountryCode = user.UserInfo.Country.Code;
            }

            if (user.UserInfo.Gender != null)
            {
                userModel.GenderLabel = user.UserInfo.Gender.Name;
            }

            return userModel;
        }
    }
}
