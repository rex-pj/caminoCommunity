using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Identity.Models
{
    public class RegisterModel
    {
        public long Id { get; set; }
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
        public bool IsActived { get; set; }
        [Required]
        public byte StatusId { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }
    }
}
