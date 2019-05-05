using Coco.Entities.Base;
using Coco.Entities.Domain.Account;
using System;
using System.Collections.Generic;

namespace Coco.Entities.Domain.Work
{
    public class Career : BaseEntity
    {
        public Career()
        {
            this.UserCareers = new HashSet<UserCareer>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }

        public virtual ICollection<UserCareer> UserCareers { get; set; }
    }
}
