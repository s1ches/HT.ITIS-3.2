using Microsoft.AspNetCore.Mvc;
using Orders.Management.Requests.Order.CreateOrder;
using Orders.Management.Services;

namespace Orders.Management.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderManager orderManager) : ControllerBase
{
    [HttpPost]
    public async Task<Guid> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        return await orderManager.CreateOrderAsync(request, cancellationToken);
    }
}