using Microsoft.EntityFrameworkCore;
using Orders.Management.Data;
using Orders.Management.Data.Entities;
using Orders.Management.Requests.Order.CreateOrder;

namespace Orders.Management.Services;

public class OrderManager(DataContext dataContext) : IOrderManager
{
    public async Task<Guid> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var orderId = Guid.NewGuid();

        var order = new Order
        {
            Id = orderId,
            CustomerId = request.CustomerId,
            ProductId = request.ProductId,
            Address = request.Address,
            ProductsCount = request.ProductsCount,
            CreateDate = DateTime.UtcNow
        };

        dataContext.Orders.Add(order);
        await dataContext.SaveChangesAsync(cancellationToken);

        return orderId;
    }
}