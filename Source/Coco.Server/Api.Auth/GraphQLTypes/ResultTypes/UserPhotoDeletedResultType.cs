using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Entities.Dtos.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ApiUserPhotoDeletedResultType: ApiResultType<UpdateUserPhotoDto, UserPhotoDeletedResultType>
    {

    }

    public class UserPhotoDeletedResultType : ObjectGraphType<UpdateUserPhotoDto>
    {
        public UserPhotoDeletedResultType()
        {
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
