using LinqToDB.Mapping;

namespace Coco.Data.MapBuilders
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
