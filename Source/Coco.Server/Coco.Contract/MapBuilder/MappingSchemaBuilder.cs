using LinqToDB.Mapping;

namespace Coco.Contract.MapBuilder
{
    public class MappingSchemaBuilder
    {
        public FluentMappingBuilder FluentMappingBuilder { get; }
        public MappingSchemaBuilder(FluentMappingBuilder fluentMappingBuilder)
        {
            FluentMappingBuilder = fluentMappingBuilder;
        }

        public void ApplyMappingBuilder<TEntity>(EntityTypeBuilder<TEntity> entityTypeBuilder)
        {
            entityTypeBuilder.Configure(FluentMappingBuilder);
        }
    }
}
