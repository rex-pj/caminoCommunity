using Microsoft.Extensions.DependencyInjection;
using System;

namespace Camino.Framework.Providers.Contracts
{
    public interface IConfigureServicesAction
    {
        int Priority { get; }

        void Execute(IServiceCollection services, IServiceProvider serviceProvider);
    }
}
