using Coco.Core.Models;
using HotChocolate.Types;

namespace Coco.Framework.GraphQLTypes.ResultTypes
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
