using Coco.Api.Framework.Commons.Enums;
using Coco.Api.Framework.Models;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IPasswordHasher<TUser> where TUser : class
    {
        string HashPassword(ApplicationUser user, string password);
        PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword);
    }
}
