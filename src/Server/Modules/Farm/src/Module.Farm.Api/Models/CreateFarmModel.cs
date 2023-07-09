using System.ComponentModel.DataAnnotations;

namespace Module.Farm.Api.Models
{
    public class CreateFarmModel
    {
        public CreateFarmModel()
        {
        }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Description { get; set; }
        [Required]
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
    }
}
