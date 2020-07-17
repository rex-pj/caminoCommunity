using Microsoft.AspNetCore.Builder;
using System;

namespace Camino.Framework.Providers.Contracts
{
    public interface IModularConfigureAction
    {
        int Priority { get; }

        void Execute(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider);
    }
}
