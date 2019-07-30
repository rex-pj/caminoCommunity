using Coco.Entities.Model.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class DeleteUserPhotoInputType : InputObjectGraphType<UpdateUserPhotoModel>
    {
        public DeleteUserPhotoInputType()
        {
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
