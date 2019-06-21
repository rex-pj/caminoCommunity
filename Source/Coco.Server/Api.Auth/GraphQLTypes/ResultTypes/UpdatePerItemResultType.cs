using Api.Auth.GraphQLTypes.InputTypes;
using Coco.Api.Framework.Models;
using GraphQL.Types;

namespace Api.Auth.GraphQLTypes.ResultTypes
{
    public class UpdatePerItemResultType : ObjectGraphType<UpdatePerItemResultModel>
    {
        public UpdatePerItemResultType()
        {
            Field(x => x.Result, type: typeof(ItemUpdatedResultType));
            Field(x => x.Errors, type: typeof(ListGraphType<IdentityErrorType>));
            Field(x => x.IsSuccess, type: typeof(BooleanGraphType));
        }
    }
}
