using Coco.Entities.Model.General;
using GraphQL.Types;

namespace Api.Identity.GraphQLTypes.ResultTypes
{
    public class UserPhotoDeletedResultType : ObjectGraphType<UpdateUserPhotoModel>
    {
        public UserPhotoDeletedResultType()
        {
            Field(x => x.CanEdit, false, typeof(BooleanGraphType));
        }
    }
}
