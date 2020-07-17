namespace Coco.Core.Dtos.Identity
{
    public class UserPasswordUpdateDto
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
