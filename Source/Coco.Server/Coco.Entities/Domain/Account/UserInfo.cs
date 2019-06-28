using Coco.Entities.Base;
using Coco.Entities.Domain.Dbo;
using System;
using System.ComponentModel.DataAnnotations;

namespace Coco.Entities.Domain.Account
{
    public class UserInfo : BaseEntity
    {
        public long Id { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public byte? GenderId { get; set; }
        public int? CountryId { get; set; }
        public virtual User User { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }
    }
}
