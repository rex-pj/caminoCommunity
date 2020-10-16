using Camino.Core.Modular.Contracts;
using Camino.Framework.GraphQL.DirectiveTypes;
using HotChocolate.Types;
using Module.Api.Feed.GraphQL.InputTypes;
using Module.Api.Feed.GraphQL.Resolvers.Contracts;
using Module.Api.Feed.GraphQL.ResultTypes;

namespace Module.Api.Feed.GraphQL
{
    public class QueryType : BaseQueryType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field<IFeedResolver>(x => x.GetUserFeedsAsync(default))
                .Type<FeedPageListType>()
                .Directive<AuthenticationDirectiveType>()
                .Argument("criterias", a => a.Type<FeedFilterInputType>());

            descriptor.Field<IFeedResolver>(x => x.GetFeedsAsync(default))
                .Type<FeedPageListType>()
                .Argument("criterias", a => a.Type<FeedFilterInputType>());
        }
    }
}
