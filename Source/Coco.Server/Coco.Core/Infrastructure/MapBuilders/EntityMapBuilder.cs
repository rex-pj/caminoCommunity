using LinqToDB.Mapping;

namespace Coco.Core.Infrastructure.MapBuilders
{
    public abstract class EntityMapBuilder
    {
        protected EntityMapBuilder()
        {

        }

        public abstract void Map(FluentMappingBuilder builder);
    }

    public abstract class EntityMapBuilder<TEntity>: EntityMapBuilder
    {
    }
}
