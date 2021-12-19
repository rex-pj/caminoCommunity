using Camino.Core.Domain.Orders;
using Camino.Infrastructure.Commons.Constants;
using Camino.Infrastructure.MapBuilders;
using LinqToDB.Mapping;

namespace Camino.Infrastructure.Mapping.Products
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
