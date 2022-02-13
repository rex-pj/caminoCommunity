using Camino.Core.Domain.Orders;
using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class OrderMap : EntityMapBuilder<Order>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<Order>()
                .HasTableName(nameof(Order))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.OrderItems,
                    (order, orderItem) => order.Id == orderItem.OrderId);
        }
    }
}
