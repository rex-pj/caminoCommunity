using Api.Public.Mutations;
using Api.Public.Queries;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Public.GraphQLSchemas
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
