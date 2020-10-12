using Camino.Framework.GraphQL.InputTypes;
using HotChocolate.Types;
using Module.Api.Farm.Models;

namespace Module.Api.Farm.GraphQL.InputTypes
{
    public class FarmInputType : InputObjectType<FarmModel>
    {
        protected override void Configure(IInputObjectTypeDescriptor<FarmModel> descriptor)
        {
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.FarmTypeId).Type<LongType>();
            descriptor.Field(x => x.Thumbnails).Type<ListType<PictureRequestInputType>>();
            descriptor.Field(x => x.Id).Ignore();
            descriptor.Field(x => x.Description).Ignore();
            descriptor.Field(x => x.UpdateById).Ignore();
            descriptor.Field(x => x.UpdatedDate).Ignore();
            descriptor.Field(x => x.CreatedById).Ignore();
            descriptor.Field(x => x.CreatedDate).Ignore();
            descriptor.Field(x => x.CreatedBy).Ignore();
            descriptor.Field(x => x.UpdatedBy).Ignore();
            descriptor.Field(x => x.FarmTypeName).Ignore();
        }
    }
}
