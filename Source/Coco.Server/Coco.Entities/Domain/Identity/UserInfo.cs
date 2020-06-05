using System;
using System.ComponentModel.DataAnnotations;

namespace Coco.Entities.Domain.Identity
{
    public class UserInfo
    {
        public UserInfo()
        {
        }

        public long Id { get; set; }
        
        [Phone]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        
        [Required]
        public DateTime? BirthDate { get; set; }
        public byte? GenderId { get; set; }
        public short? CountryId { get; set; }
        public virtual User User { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }
    }
}
