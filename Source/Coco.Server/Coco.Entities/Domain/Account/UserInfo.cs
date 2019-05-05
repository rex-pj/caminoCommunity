using Coco.Entities.Base;
using Coco.Entities.Domain.Dbo;
using System;

namespace Coco.Entities.Domain.Account
{
    public class UserInfo : BaseEntity
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public byte? GenderId { get; set; }
        public int? CountryId { get; set; }
        public bool IsActived { get; set; }
        public byte StatusId { get; set; }
        public virtual User User { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Country Country { get; set; }
        public virtual Status Status { get; set; }
    }
}
