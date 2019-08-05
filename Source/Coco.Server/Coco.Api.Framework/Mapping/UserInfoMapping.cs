using Coco.Api.Framework.Models;
using Coco.Entities.Model.User;

namespace Coco.Api.Framework.Mapping
{
    public static class UserInfoMapping
    {
        public static UserInfo ApplicationUserToUserInfo(ApplicationUser user, string userIdentityId)
        {
            return new UserInfo()
            {
                UserIdentityId = userIdentityId,
                Email = user.Email,
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                DisplayName = user.DisplayName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                BirthDate = user.BirthDate,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.CreatedDate,
                GenderId = user.GenderId,
                CountryId = user.CountryId,
                IsActived = user.IsActived,
                StatusId = user.StatusId,
                CountryCode = user.CountryCode,
                GenderLabel = user.GenderLabel,
                CountryName = user.CountryName,
                AvatarUrl = user.AvatarUrl,
                CoverPhotoUrl = user.CoverPhotoUrl
            };
        }

        public static ApplicationUser PopulateApplicationUser(UserModel userModel)
        {
            if (userModel == null) {
                return null;
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = userModel.Email,
                Id = userModel.Id,
                UserName = userModel.Email,
                Lastname = userModel.Lastname,
                Firstname = userModel.Firstname,
                Password = userModel.Password,
                PasswordHash = userModel.Password,
                PasswordSalt = userModel.PasswordSalt,
                PhoneNumber = userModel.PhoneNumber,
                Address = userModel.Address,
                BirthDate = userModel.BirthDate,
                CountryId = userModel.CountryId,
                CreatedById = userModel.CreatedById,
                Description = userModel.Description,
                DisplayName = userModel.DisplayName,
                GenderId = userModel.GenderId,
                IsActived = userModel.IsActived,
                StatusId = userModel.StatusId,
                UpdatedById = userModel.UpdatedById,
                Expiration = userModel.Expiration,
                AuthenticationToken = userModel.AuthenticationToken,
                SecurityStamp = userModel.SecurityStamp,
                AvatarUrl = userModel.AvatarUrl,
                CoverPhotoUrl = userModel.CoverPhotoUrl
            };

            return applicationUser;
        }

        public static ApplicationUser PopulateLoggedInUser(UserLoggedInModel userModel)
        {
            if (userModel == null)
            {
                return null;
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = userModel.Email,
                Id = userModel.Id,
                UserName = userModel.Email,
                Lastname = userModel.Lastname,
                Firstname = userModel.Firstname,
                Password = userModel.Password,
                PasswordHash = userModel.Password,
                PasswordSalt = userModel.PasswordSalt,
                CountryId = userModel.CountryId,
                Description = userModel.Description,
                DisplayName = userModel.DisplayName,
                GenderId = userModel.GenderId,
                IsActived = userModel.IsActived,
                StatusId = userModel.StatusId,
                Expiration = userModel.Expiration,
                AuthenticationToken = userModel.AuthenticationToken,
                AvatarUrl = userModel.AvatarUrl,
                CoverPhotoUrl = userModel.CoverPhotoUrl
            };

            return applicationUser;
        }

        public static UserModel PopulateUserEntity(ApplicationUser loggedUser)
        {
            UserModel userModel = new UserModel()
            {
                Email = loggedUser.Email,
                Id = loggedUser.Id,
                Address = loggedUser.Address,
                BirthDate = loggedUser.BirthDate,
                CountryId = loggedUser.CountryId,
                CreatedById = loggedUser.CreatedById,
                Description = loggedUser.Description,
                DisplayName = loggedUser.DisplayName,
                Firstname = loggedUser.Firstname,
                Lastname = loggedUser.Lastname,
                GenderId = loggedUser.GenderId,
                IsActived = loggedUser.IsActived,
                Password = loggedUser.PasswordHash,
                PasswordSalt = loggedUser.PasswordSalt,
                PhoneNumber = loggedUser.PhoneNumber,
                StatusId = loggedUser.StatusId,
                UpdatedById = loggedUser.UpdatedById,
                Expiration = loggedUser.Expiration,
                AuthenticationToken = loggedUser.AuthenticationToken,
                SecurityStamp = loggedUser.SecurityStamp
            };

            return userModel;
        }

        public static UserInfoExt FullUserModelToInfo(UserFullModel user)
        {
            var userInfo = new UserInfo()
            {
                Email = user.Email,
                Lastname = user.Lastname,
                Firstname = user.Firstname,
                DisplayName = user.DisplayName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                BirthDate = user.BirthDate,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.CreatedDate,
                GenderId = user.GenderId,
                CountryId = user.CountryId,
                IsActived = user.IsActived,
                StatusId = user.StatusId,
                CountryCode = user.CountryCode,
                CountryName = user.CountryName,
                GenderLabel = user.GenderLabel,
                StatusLabel = user.StatusLabel,
                AvatarUrl = user.AvatarUrl,
                CoverPhotoUrl = user.CoverPhotoUrl
            };

            var result = new UserInfoExt(userInfo);
            return result;
        }

        public static UserProfileUpdateModel UserProfileUpdateModel(ApplicationUser user)
        {
            UserProfileUpdateModel userModel = new UserProfileUpdateModel()
            {
                DisplayName = user.DisplayName,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Id = user.Id
            };

            return userModel;
        }
    }
}
