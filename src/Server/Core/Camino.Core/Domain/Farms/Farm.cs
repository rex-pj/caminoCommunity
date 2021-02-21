using System;

namespace Camino.Core.Domain.Farms
{
    public class Farm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
        public int StatusId { get; set; }
        public virtual FarmType FarmType { get; set; }
    }
}
