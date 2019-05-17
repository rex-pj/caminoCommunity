using Coco.Api.Framework.Models;

namespace Coco.Api.Framework.AccountIdentity.Contracts
{
    public interface IPasswordHasher<TUser> where TUser : class
    {
        string HashPassword(ApplicationUser user, string password);
    }
}
