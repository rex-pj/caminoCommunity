using System;

namespace Camino.DAL.Entities
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
    }
}
