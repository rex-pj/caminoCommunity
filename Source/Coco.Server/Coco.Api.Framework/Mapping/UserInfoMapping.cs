using Coco.Api.Framework.Models;

namespace Coco.Api.Framework.Mapping
{
    public static class UserInfoMapping
    {
        public static UserInfo ApplicationUserToUserInfo(ApplicationUser user, string userHashedId)
        {
            return new UserInfo()
            {
                UserHashedId = userHashedId,
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
                StatusId = user.StatusId
            };
        }
    }
}
