using Api.Auth.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.InputTypes
{
    public class RegisterInputType : InputObjectGraphType<RegisterModel>
    {
        public RegisterInputType()
        {
            Field(x => x.Lastname, false, typeof(StringGraphType));
            Field(x => x.Firstname, false, typeof(StringGraphType));
            Field(x => x.Email, false, typeof(StringGraphType));
            Field(x => x.Password, false, typeof(StringGraphType));
            Field(x => x.ConfirmPassword, false, typeof(StringGraphType));
            Field(x => x.GenderId, type: typeof(IntGraphType));
            Field(x => x.BirthDate, false, type: typeof(DateTimeGraphType));
        }
    }
}
