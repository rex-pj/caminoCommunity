using System;
using System.ComponentModel.DataAnnotations;

namespace  Module.Auth.Api.Models
{
    public class SignupModel
    {
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }
    }
}
