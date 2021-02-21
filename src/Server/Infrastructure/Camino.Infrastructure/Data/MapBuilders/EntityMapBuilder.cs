using LinqToDB.Mapping;

namespace Camino.Infrastructure.MapBuilders
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
