using System;

namespace Camino.Service.Projections.Product
{
    public class ProductFarmProjection
    {
        public int Id { get; set; }

        public long FarmId { get; set; }
        public string FarmName { get; set; }

        public long ProductId { get; set; }

        public DateTimeOffset LinkedDate { get; set; }

        public bool IsLinked { get; set; }

        public long LinkedById { get; set; }
        public long ApprovedById { get; set; }
    }
}
