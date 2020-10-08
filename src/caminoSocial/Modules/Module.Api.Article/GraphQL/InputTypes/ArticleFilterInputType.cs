using HotChocolate.Types;
using Module.Api.Article.Models;

namespace Module.Api.Article.GraphQL.InputTypes
{
    public class ArticleFilterInputType : InputObjectType<ArticleFilterModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ArticleFilterModel> descriptor)
        {
            descriptor.Field(x => x.Page).Type<IntType>().DefaultValue(1);
            descriptor.Field(x => x.PageSize).Type<IntType>().DefaultValue(10);
            descriptor.Field(x => x.Search).Type<StringType>();
            descriptor.Field(x => x.UserIdentityId).Type<StringType>();
        }
    }
}
