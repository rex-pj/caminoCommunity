using HotChocolate.Types;
using Module.Api.Content.Models;

namespace Module.Api.Content.GraphQL.InputTypes
{
    public class ArticleFilterInputType : InputObjectType<ArticleFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ArticleFilterModel> descriptor)
        {
            descriptor.Field(x => x.Page).Type<IntType>();
            descriptor.Field(x => x.PageSize).Type<IntType>();
            descriptor.Field(x => x.Search).Type<StringType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
        }
    }
}
