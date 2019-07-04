using Api.Identity.Models;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UserInfoUpdateInputType : InputObjectGraphType<UserInfoUpdateModel>
    {
        public UserInfoUpdateInputType()
        {
            Field(x => x.Lastname, false, typeof(StringGraphType));
            Field(x => x.Firstname, false, typeof(StringGraphType));
            Field(x => x.Email, false, typeof(StringGraphType));
            Field(x => x.GenderId, type: typeof(IntGraphType));
            Field(x => x.BirthDate, false, type: typeof(DateTimeGraphType));
            Field(x => x.Description, false, type: typeof(StringGraphType));
            Field(x => x.Address, false, type: typeof(StringGraphType));
            Field(x => x.CountryId, false, type: typeof(InterfaceGraphType));
            Field(x => x.PhoneNumber, false, type: typeof(StringGraphType));
        }
    }
}
