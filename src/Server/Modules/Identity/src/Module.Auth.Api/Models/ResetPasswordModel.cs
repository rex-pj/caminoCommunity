using System.ComponentModel.DataAnnotations;

namespace Module.Auth.Api.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Key { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
