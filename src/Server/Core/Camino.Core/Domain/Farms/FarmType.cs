using System;

namespace Camino.Core.Domain.Farms
{
    public class FarmType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public long UpdatedById { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public long CreatedById { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublished { get; set; }
    }
}
