using Coco.Entities.Domain.Account;
using Coco.Entities.Model.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Business.Mapping
{
    public static class UserMapping
    {
        public static UserInfo UserModelToEntity(UserModel userModel)
        {
            UserInfo user = new UserInfo
            {
                GenderId = userModel.GenderId,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                UpdatedById = userModel.UpdatedById,
                CreatedById = userModel.CreatedById,
                Address = userModel.Address,
                BirthDate = userModel.BirthDate,
                CountryId = userModel.CountryId,
                Description = userModel.Description,
                IsActived = false,
                PhoneNumber = userModel.PhoneNumber,
                StatusId = 1,
                User = new User()
                {
                    DisplayName = userModel.DisplayName,
                    Email = userModel.Email,
                    Firstname = userModel.Firstname,
                    Lastname = userModel.Lastname,
                    Password = userModel.Password,
                    PasswordSalt = userModel.PasswordSalt,
                    AuthenticatorToken = userModel.AuthenticatorToken,
                    SecurityStamp = userModel.SecurityStamp,
                    Expiration = userModel.Expiration
                }
            };

            return user;
        }

        public static UserModel UserEntityToModel(UserInfo user)
        {
            if (user == null)
            {
                return null;
            }

            UserModel userModel = new UserModel
            {
                GenderId = user.GenderId,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate,
                UpdatedById = user.UpdatedById,
                CreatedById = user.CreatedById,
                Address = user.Address,
                BirthDate = user.BirthDate,
                CountryId = user.CountryId,
                Description = user.Description,
                IsActived = user.IsActived,
                PhoneNumber = user.PhoneNumber,
                StatusId = user.StatusId,
                Id = user.Id
            };

            if (user.User != null)
            {
                userModel.DisplayName = user.User.DisplayName;
                userModel.Email = user.User.Email;
                userModel.Firstname = user.User.Firstname;
                userModel.Lastname = user.User.Lastname;
                userModel.Password = user.User.Password;
                userModel.PasswordSalt = user.User.PasswordSalt;
                userModel.Expiration = user.User.Expiration;
                userModel.AuthenticatorToken = user.User.AuthenticatorToken;
                userModel.SecurityStamp = user.User.SecurityStamp;
            }

            return userModel;
        }

        public static UserFullModel FullUserEntityToModel(UserInfo user)
        {
            if (user == null)
            {
                return null;
            }

            UserFullModel userModel = new UserFullModel
            {
                GenderId = user.GenderId,
                UpdatedDate = user.UpdatedDate,
                CreatedDate = user.CreatedDate,
                UpdatedById = user.UpdatedById,
                CreatedById = user.CreatedById,
                Address = user.Address,
                BirthDate = user.BirthDate,
                CountryId = user.CountryId,
                Description = user.Description,
                IsActived = user.IsActived,
                PhoneNumber = user.PhoneNumber,
                StatusId = user.StatusId,
                Id = user.Id
            };

            if(user.Country != null)
            {
                userModel.CountryName = user.Country.Name;
                userModel.CountryCode = user.Country.Code;
            }

            if(user.Gender != null)
            {
                userModel.GenderLabel = user.Gender.Name;
            }

            if(user.Status != null)
            {
                userModel.StatusLabel = user.Status.Name;
            }

            if (user.User != null)
            {
                userModel.DisplayName = user.User.DisplayName;
                userModel.Email = user.User.Email;
                userModel.Firstname = user.User.Firstname;
                userModel.Lastname = user.User.Lastname;
                userModel.Password = user.User.Password;
                userModel.PasswordSalt = user.User.PasswordSalt;
                userModel.Expiration = user.User.Expiration;
                userModel.AuthenticatorToken = user.User.AuthenticatorToken;
                userModel.SecurityStamp = user.User.SecurityStamp;
            }

            return userModel;
        }
    }
}
