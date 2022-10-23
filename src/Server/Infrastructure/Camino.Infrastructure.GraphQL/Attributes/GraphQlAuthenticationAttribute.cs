using Camino.Infrastructure.GraphQL.DirectiveTypes;
using HotChocolate.Types.Descriptors;
using System.Reflection;

namespace Camino.Infrastructure.GraphQL.Attributes
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
