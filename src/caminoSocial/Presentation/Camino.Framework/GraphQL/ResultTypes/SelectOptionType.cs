using Camino.Core.Models;
using HotChocolate.Types;

namespace Camino.Framework.GraphQL.ResultTypes
{
    public class SelectOptionType<T> : ObjectType<T> where T : ISelectOption
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            base.Configure(descriptor);
            descriptor.Field(x => x.Id).Type<StringType>();
            descriptor.Field(x => x.Text).Type<StringType>();
            descriptor.Field(x => x.IsSelected).Type<BooleanType>();
        }
    }

    public class SelectOptionType : SelectOptionType<ISelectOption>
    {
        public SelectOptionType() : base() {}
    }
}
