using Camino.Core.Domain.Orders;
using Camino.Shared.Constants;
using Camino.Infrastructure.Linq2Db.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Linq2Db.Mapping.Products
{
    public class OrderItemMap : EntityMapBuilder<OrderItem>
    {
        public override void Map(FluentMappingBuilder builder)
        {
            builder.Entity<OrderItem>()
                .HasTableName(nameof(OrderItem))
                .HasSchemaName(TableSchemaConst.Dbo)
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.Order,
                    (orderItem, order) => orderItem.OrderId == order.Id);
        }
    }
}
