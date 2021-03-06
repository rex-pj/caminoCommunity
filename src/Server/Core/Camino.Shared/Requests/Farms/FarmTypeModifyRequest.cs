﻿namespace Camino.Shared.Requests.Farms
{
    public class FarmTypeModifyRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long UpdatedById { get; set; }

        public long CreatedById { get; set; }
    }
}
