using Microsoft.AspNetCore.Mvc;
using Orders.Delivery.Data.Enums;

namespace Orders.Delivery.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryController : ControllerBase
{
    [HttpPatch]
    public Task<Guid> UpdateDeliveryState(Guid deliveryId, DeliveryState deliveryState)
    {
        // Logic.
        return Task.FromResult(deliveryId);
    }
}