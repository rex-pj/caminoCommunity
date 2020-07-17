using System.ComponentModel.DataAnnotations.Schema;

namespace Camino.Data.Entities.Content
{
    public class UserCareer
    {
        [ForeignKey("Career")]
        public byte CareerId { get; set; }

        public long UserId { get; set; }

        public virtual Career Career { get; set; }
    }
}
