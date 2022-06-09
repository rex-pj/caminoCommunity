using Camino.Core.Domains.Products;
using System;

namespace Camino.Core.Domains.Farms
{
    public class FarmProduct
    {
        public int Id { get; set; }

        public long FarmId { get; set; }

        public long ProductId { get; set; }

        public DateTimeOffset LinkedDate { get; set; }

        public bool IsLinked { get; set; }

        public long LinkedById { get; set; }
        public long ApprovedById { get; set; }
        public virtual Product Product { get; set; }
        public virtual Farm Farm { get; set; }
    }
}
