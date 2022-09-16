namespace Camino.Core.Domains.Farms
{
    public class FarmType
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime UpdatedDate { get; set; }

        public long UpdatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedById { get; set; }
        public int StatusId { get; set; }
        public virtual ICollection<Farm> Farms { get; set; }
    }
}
