using Coco.Entities.Model.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class AvatarDeletedResultType : ObjectGraphType<UpdateAvatarModel>
    {
        public AvatarDeletedResultType()
        {
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
