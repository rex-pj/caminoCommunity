using System;
using System.Collections.Generic;

namespace Camino.Core.Domains.Farms
{
    public class Farm
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
        public int StatusId { get; set; }
        public virtual FarmType FarmType { get; set; }
    }
}
