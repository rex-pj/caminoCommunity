using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Public.GraphQLTypes.InputTypes
{
    public class ResetPasswordInputType : InputObjectGraphType<ResetPasswordModel>
    {
        public ResetPasswordInputType()
        {
            Field(x => x.Email, false, typeof(StringGraphType));
            Field(x => x.ConfirmPassword, false, typeof(StringGraphType));
            Field(x => x.CurrentPassword, false, typeof(StringGraphType));
            Field(x => x.Password, false, typeof(StringGraphType));
            Field(x => x.Key, false, typeof(StringGraphType));
        }
    }
}
