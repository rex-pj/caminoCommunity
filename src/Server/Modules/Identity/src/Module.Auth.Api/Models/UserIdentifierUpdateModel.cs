using System.ComponentModel.DataAnnotations;

namespace Module.Auth.Api.Models
{
    public class UserIdentifierUpdateModel
    {
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
