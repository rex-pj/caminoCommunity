namespace Module.Auth.Api.Models
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Key { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
