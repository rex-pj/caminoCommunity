﻿using System;

namespace Camino.Shared.Requests.Filters
{
    public class ProductCategoryFilter : BaseFilter
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }
}
