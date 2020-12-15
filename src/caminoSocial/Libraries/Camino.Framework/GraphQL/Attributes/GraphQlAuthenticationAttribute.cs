using Camino.Framework.GraphQL.DirectiveTypes;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Reflection;

namespace Camino.Framework.GraphQL.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GraphQlAuthenticationAttribute : DescriptorAttribute
    {
        protected override void TryConfigure(IDescriptorContext context, IDescriptor descriptor, ICustomAttributeProvider element)
        {
            if (descriptor is IObjectFieldDescriptor field)
            {
                field.Directive<AuthenticationDirectiveType>();
            }
        }
    }
}
