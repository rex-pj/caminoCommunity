using Coco.Api.Framework.SessionManager.Contracts;
using Coco.Api.Framework.SessionManager.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Coco.Api.Framework.SessionManager.Providers
{
    public class IdentityBuilder
    {
        public Type UserType { get; private set; }

        public IServiceCollection Services { get; private set; }


        /// <summary>
        /// Creates a new instance of <see cref="IdentityBuilder"/>.
        /// </summary>
        /// <param name="user">The <see cref="Type"/> to use for the users.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(Type user, IServiceCollection services)
        {
            UserType = user;
            Services = services;
        }

        /// <summary>
        /// Adds a token provider for the <seealso cref="UserType"/>.
        /// </summary>
        /// <param name="providerName">The name of the provider to add.</param>
        /// <param name="provider">The type of the <see cref="IUserTwoFactorTokenProvider{TUser}"/> to add.</param>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddTokenProvider(string providerName, Type provider)
        {
            if (!typeof(IUserTokenProvider<>).MakeGenericType(UserType).GetTypeInfo().IsAssignableFrom(provider.GetTypeInfo()))
            {
                throw new InvalidOperationException($"{nameof(provider.Name)} IUserTwoFactorTokenProvider {UserType.Name}");
            }
            Services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap[providerName] = new TokenProviderDescriptor(provider);
            });
            Services.AddTransient(provider);
            return this;
        }
    }
}
