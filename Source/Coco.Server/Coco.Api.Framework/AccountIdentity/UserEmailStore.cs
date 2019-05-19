using Coco.Api.Framework.Models;
using Coco.Api.Framework.AccountIdentity.Contracts;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Coco.Api.Framework.AccountIdentity
{
    public class UserEmailStore : IUserEmailStore<ApplicationUser>
    {
        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Email);
        }
    }
}
