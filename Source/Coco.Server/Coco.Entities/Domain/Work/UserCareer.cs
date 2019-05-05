using Coco.Entities.Base;
using Coco.Entities.Domain.Account;

namespace Coco.Entities.Domain.Work
{
    public class UserCareer : BaseEntity
    {
        public byte CareerId { get; set; }
        public long UserId { get; set; }
        public virtual Career Career { get; set; }
        public virtual User User { get; set; }
    }
}
