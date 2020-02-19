using Api.Identity.Mutations;
using Api.Identity.Queries;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Identity.GraphQLSchemas
{
    public class UserSchema : Schema
    {
        public UserSchema(IServiceProvider provider) : base(provider)
        {
            Mutation = provider.GetRequiredService<UserMutation>();
            Query = provider.GetRequiredService<UserQuery>();
        }
    }
}
