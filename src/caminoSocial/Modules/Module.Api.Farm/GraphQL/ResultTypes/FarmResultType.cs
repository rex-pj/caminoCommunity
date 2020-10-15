using HotChocolate.Types;
using Module.Api.Farm.Models;

namespace Module.Api.Farm.GraphQL.ResultTypes
{
    public class FarmResultType : ObjectType<FarmModel>
    {
        protected override void Configure(IObjectTypeDescriptor<FarmModel> descriptor)
        {
            descriptor.Field(x => x.Id).Type<LongType>();
            descriptor.Field(x => x.Name).Type<StringType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.Address).Type<StringType>();
            descriptor.Field(x => x.CreatedByIdentityId).Type<StringType>();
            descriptor.Field(x => x.CreatedByPhotoCode).Type<StringType>();
            descriptor.Field(x => x.FarmTypeId).Type<LongType>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.UpdateById).Type<LongType>();
            descriptor.Field(x => x.UpdatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedById).Type<LongType>();
            descriptor.Field(x => x.CreatedDate).Type<DateTimeType>();
            descriptor.Field(x => x.CreatedBy).Type<StringType>();
            descriptor.Field(x => x.UpdatedBy).Type<StringType>();
            descriptor.Field(x => x.FarmTypeName).Type<StringType>();
            descriptor.Field(x => x.CreatedByPhotoCode).Type<StringType>();
        }
    }
}
