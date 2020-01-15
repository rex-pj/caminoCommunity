using Coco.Entities.Dtos.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class DeleteUserPhotoInputType : InputObjectGraphType<UpdateUserPhotoDto>
    {
        public DeleteUserPhotoInputType()
        {
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
