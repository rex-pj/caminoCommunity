using Coco.Api.Framework.SessionManager.Core;
using Coco.Api.Framework.SessionManager.Providers;

namespace Coco.Api.Framework.Infrastructure.Extensions
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddDefaultTokenProviders(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProviderType);
        }
    }
}
