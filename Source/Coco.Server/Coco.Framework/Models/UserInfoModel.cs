using System;

namespace Coco.Framework.Models
{
    public class UserInfoModel
    {
        public UserInfoModel()
        {

        }

        public UserInfoModel(UserInfoModel userInfo)
        {
            Address = userInfo.Address;
            UserIdentityId = userInfo.UserIdentityId;
            Email = userInfo.Email;
            Lastname = userInfo.Lastname;
            Firstname = userInfo.Firstname;
            DisplayName = userInfo.DisplayName;
            Address = userInfo.Address;
            PhoneNumber = userInfo.PhoneNumber;
            Description = userInfo.Description;
            BirthDate = userInfo.BirthDate;
            CreatedDate = userInfo.CreatedDate;
            UpdatedDate = userInfo.UpdatedDate;
            GenderId = userInfo.GenderId;
            GenderLabel = userInfo.GenderLabel;
            IsActived = userInfo.IsActived;
            StatusId = userInfo.StatusId;
            StatusLabel = userInfo.StatusLabel;
            CountryId = userInfo.CountryId;
            CountryCode = userInfo.CountryCode;
            CountryName = userInfo.CountryName;
        }

        public string UserIdentityId { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? GenderId { get; set; }
        public string GenderLabel { get; set; }
        public bool IsActived { get; set; }
        public int StatusId { get; set; }
        public string StatusLabel { get; set; }
        public short? CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
