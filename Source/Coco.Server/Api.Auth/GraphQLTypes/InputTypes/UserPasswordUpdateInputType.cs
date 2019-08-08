using Coco.Entities.Model.User;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UserPasswordUpdateInputType : InputObjectGraphType<UserPasswordUpdateModel>
    {
        public UserPasswordUpdateInputType()
        {
            Field(x => x.UserId, false, typeof(FloatGraphType));
            Field(x => x.ConfirmPassword, false, typeof(StringGraphType));
            Field(x => x.NewPassword, false, typeof(StringGraphType));
            Field(x => x.CurrentPassword, type: typeof(StringGraphType));
        }
    }
}
