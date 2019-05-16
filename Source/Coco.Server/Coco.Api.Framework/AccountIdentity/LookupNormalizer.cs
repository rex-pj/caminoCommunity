using Coco.Api.Framework.AccountIdentity.Contracts;

namespace Coco.Api.Framework.AccountIdentity
{
    public sealed class LookupNormalizer: ILookupNormalizer
    {
        /// <summary>
        /// Returns a normalized representation of the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The key to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="name"/>.</returns>
        public string NormalizeName(string name)
        {
            if (name == null)
            {
                return null;
            }
            return name.Normalize();
        }

        /// <summary>
        /// Returns a normalized representation of the specified <paramref name="email"/>.
        /// </summary>
        /// <param name="email">The email to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="email"/>.</returns>
        public string NormalizeEmail(string email) => NormalizeName(email);
    }
}
