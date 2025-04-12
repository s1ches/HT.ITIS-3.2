using Orders.Management.Requests.Order.CreateOrder;

namespace Orders.Management.Services;

public interface IOrderManager
{
    Task<Guid> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
}