using Orders.Common.Abstractions;
using Orders.Common.Enums;
using Orders.Common.Messages;

namespace Orders.Management.Data.Entities;

public class Order : IOutboxMessageEntity
{
    public Guid Id { get; set; }
    
    public int ProductsCount { get; set; }
    
    public Guid ProductId { get; set; }

    public Guid CustomerId { get; set; }
    
    public string Address { get; set; } = string.Empty;
    
    public DateTime CreateDate { get; set; }
    
    public IOutboxMessage Message =>
        new OrderCreatedMessage
        {
            Id = Guid.NewGuid(),
            ProductId = ProductId,
            CustomerId = CustomerId,
            Address = Address,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            ProductsCount = ProductsCount,
            OrderId = Id,
            State = OutboxMessageState.Pending
        };
}