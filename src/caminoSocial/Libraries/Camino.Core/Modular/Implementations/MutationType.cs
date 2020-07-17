using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Core.Modular.Implementations
{
    public class MutationType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var baseMutationType = typeof(BaseMutationType);

            foreach (var module in modules)
            {
                var mutationType = module.Assembly.GetTypes().FirstOrDefault(x => baseMutationType.IsAssignableFrom(x));
                if (mutationType != null && mutationType != baseMutationType)
                {
                    var mutation = Activator.CreateInstance(mutationType) as BaseMutationType;
                    mutation.Register(descriptor);
                }
            }
        }
    }
}
