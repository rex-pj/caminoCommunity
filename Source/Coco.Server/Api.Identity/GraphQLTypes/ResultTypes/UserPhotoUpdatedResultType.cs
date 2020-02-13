using Coco.Api.Framework.GraphQLTypes.ResultTypes;
using Coco.Entities.Dtos.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class ApiUserPhotoUpdatedResultType: ApiResultType<UpdateUserPhotoDto, UserPhotoUpdatedResultType>
    {

    }

    public class UserPhotoUpdatedResultType : ObjectGraphType<UpdateUserPhotoDto>
    {
        public UserPhotoUpdatedResultType()
        {
            Field(x => x.ContentType, false, typeof(StringGraphType));
            Field(x => x.Height, false, typeof(FloatGraphType));
            Field(x => x.Width, false, typeof(FloatGraphType));
            Field(x => x.XAxis, false, typeof(FloatGraphType));
            Field(x => x.YAxis, false, typeof(FloatGraphType));
            Field(x => x.PhotoUrl, false, typeof(StringGraphType));
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
