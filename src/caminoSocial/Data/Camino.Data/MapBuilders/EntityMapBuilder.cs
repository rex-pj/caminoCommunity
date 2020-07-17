using LinqToDB.Mapping;

namespace Camino.Data.MapBuilders
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
