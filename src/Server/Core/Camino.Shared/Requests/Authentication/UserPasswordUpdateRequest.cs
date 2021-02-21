namespace Camino.Shared.Requests.Authentication
{
    public class UserPasswordUpdateRequest
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
