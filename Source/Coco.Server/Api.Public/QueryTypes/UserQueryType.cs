using Api.Public.GraphQLTypes.InputTypes;
using Api.Public.Resolvers.Contracts;
using HotChocolate.Types;

namespace Api.Public.QueryTypes
{
    public class UserQueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("loggedUser")
                .Resolver(ctx => ctx.Service<IUserResolver>().GetLoggedUser(ctx.ContextData));

            descriptor.Field("fullUserInfo")
                .Argument("criterias", a => a.Type<FindUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().GetFullUserInfoAsync(ctx));

            descriptor.Field("active")
                .Argument("criterias", a => a.Type<ActiveUserInputType>())
                .Resolver(ctx => ctx.Service<IUserResolver>().ActiveAsync(ctx));
        }

        //public UserQuery(IUserResolver userResolver)
        //{
        //    //Field<LoggedInUserResultType>("loggedUser",
        //    //    resolve: context =>
        //    //    {
        //    //        return userResolver.GetLoggedUser(context.UserContext);
        //    //    });

        //    //FieldAsync<ApiResultType<UserInfoExtend, FullUserInfoResultType>>("fullUserInfo",
        //    //    arguments: new QueryArguments(new QueryArgument<FindUserInputType> { Name = "criterias" }),
        //    //    resolve: async context =>
        //    //    {
        //    //        return await userResolver.GetFullUserInfoAsync(context);
        //    //    });

        //    //FieldAsync<ActiveUserResultType>("active",
        //    //    arguments: new QueryArguments(new QueryArgument<ActiveUserInputType> { Name = "criterias" }),
        //    //    resolve: async context =>
        //    //    {
        //    //        return await userResolver.ActiveAsync(context);
        //    //    });
        //}
    }
}
