using LinqToDB.Mapping;

namespace Coco.Core.Infrastructure.MapBuilders
{
    public static class MappingBuilderExtensions
    {
        public static FluentMappingBuilder ApplyMappingBuilder<TEntity>(this FluentMappingBuilder fluentMappingBuilder,
            EntityMapBuilder<TEntity> entityTypeBuilder)
        {
            entityTypeBuilder.Map(fluentMappingBuilder);

            return fluentMappingBuilder;
        }

        public static FluentMappingBuilder ApplyMappingBuilder<T>(this FluentMappingBuilder fluentMappingBuilder)
            where T : EntityMapBuilder, new()
        {
            var entityTypeBuilder = new T();
            entityTypeBuilder.Map(fluentMappingBuilder);

            return fluentMappingBuilder;
        }
    }
}
