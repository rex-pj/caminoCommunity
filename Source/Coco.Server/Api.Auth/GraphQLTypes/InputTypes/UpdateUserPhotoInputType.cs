using Coco.Entities.Dtos.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class UpdateUserPhotoInputType : InputObjectGraphType<UpdateUserPhotoDto>
    {
        public UpdateUserPhotoInputType()
        {
            Field(x => x.ContentType, false, typeof(StringGraphType));
            Field(x => x.Height, false, typeof(FloatGraphType));
            Field(x => x.Width, false, typeof(FloatGraphType));
            Field(x => x.Scale, false, typeof(FloatGraphType));
            Field(x => x.XAxis, false, typeof(FloatGraphType));
            Field(x => x.YAxis, false, typeof(FloatGraphType));
            Field(x => x.PhotoUrl, false, typeof(StringGraphType));
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
            Field(x => x.FileName, false, typeof(StringGraphType));
        }
    }
}
