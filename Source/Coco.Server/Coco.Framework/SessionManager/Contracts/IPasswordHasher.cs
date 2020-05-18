using Coco.Entities.Enums;
using Coco.Framework.Models;

namespace Coco.Framework.SessionManager.Contracts
{
    public interface IPasswordHasher<TUser> where TUser : class
    {
        string HashPassword(ApplicationUser user, string password);
        PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword);
    }
}
