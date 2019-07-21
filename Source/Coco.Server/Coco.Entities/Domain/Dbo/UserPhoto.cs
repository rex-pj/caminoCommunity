using Coco.Entities.Base;
using Coco.Entities.Domain.Identity;
using System;

namespace Coco.Entities.Domain.Dbo
{
    public class UserPhoto : BaseEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public string ImageData { get; set; }
        public long UserId { get; set; }
        public byte TypeId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}
