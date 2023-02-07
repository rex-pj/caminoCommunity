using System.ComponentModel.DataAnnotations;

namespace  Module.Auth.Api.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Email { get; set; }
    }
}
