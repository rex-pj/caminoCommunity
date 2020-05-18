using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coco.Entities.Domain.Content
{
    public class Career
    {
        public Career()
        {
            UserCareers = new HashSet<UserCareer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedById { get; set; }

        [ForeignKey("CareerId")]
        public virtual ICollection<UserCareer> UserCareers { get; set; }
    }
}
