using System.ComponentModel.DataAnnotations;

namespace Module.Auth.Api.Models
{
    public class UserPasswordUpdateModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
