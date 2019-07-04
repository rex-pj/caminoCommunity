using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Identity.Models
{
    public class UserInfoUpdateModel
    {
        [Required]
        public DateTime? BirthDate { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Email { get; set; }
        public int? GenderId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public short? CountryId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
