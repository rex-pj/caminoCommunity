using System.ComponentModel.DataAnnotations;

namespace Module.Web.SetupManagement.Models
{
    public class SetupModel
    {
        [Required]
        public string AdminEmail { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string AdminPassword { get; set; }
        [Required]
        public string AdminConfirmPassword { get; set; }
    }
}
