using Coco.Entities.Model.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.InputTypes
{
    public class DeleteAvatarInputType : InputObjectGraphType<UpdateAvatarModel>
    {
        public DeleteAvatarInputType()
        {
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
