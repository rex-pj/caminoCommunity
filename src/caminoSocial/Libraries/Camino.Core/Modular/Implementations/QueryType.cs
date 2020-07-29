using Camino.Core.Infrastructure;
using Camino.Core.Models;
using Camino.Core.Modular.Contracts;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Camino.Core.Modular.Implementations
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            var modules = Singleton<IList<ModuleInfo>>.Instance;
            var baseQueryType = typeof(BaseQueryType);

            foreach(var module in modules)
            {
                var queryType = module.Assembly.GetTypes().FirstOrDefault(x => baseQueryType.IsAssignableFrom(x));
                if (queryType != null && queryType != baseQueryType)
                {
                    var query = Activator.CreateInstance(queryType) as BaseQueryType;
                    query.Register(descriptor);
                }
            }
        }
    }
}
