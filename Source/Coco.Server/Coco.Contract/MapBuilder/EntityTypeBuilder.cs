using LinqToDB.Mapping;

namespace Coco.Contract.MapBuilder
{
    public abstract class EntityTypeBuilder<TEntity>
    {
        protected EntityTypeBuilder(FluentMappingBuilder fluentMappingBuilder)
        {
            Configure(fluentMappingBuilder);
        }

        public abstract void Configure(FluentMappingBuilder builder);
    }
}
