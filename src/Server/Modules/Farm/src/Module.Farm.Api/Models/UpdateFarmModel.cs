using System.ComponentModel.DataAnnotations;

namespace Module.Farm.Api.Models
{
    public class UpdateFarmModel
    {
        public UpdateFarmModel()
        {
        }

        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public long FarmTypeId { get; set; }
        public string Address { get; set; }
    }
}
