using Orders.Common.Abstractions;
using Orders.Common.Enums;

namespace Orders.Common.Messages;

public class OrderCreatedMessage : IOutboxMessage
{
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public Guid ProductId { get; set; }

    public string Address { get; set; } = string.Empty;

    public int ProductsCount { get; set; }

    public Guid Id { get; set; }
    
    public OutboxMessageState State { get; set; }
    
    public DateTime UpdateDate { get; set; }
    
    public DateTime CreateDate { get; set; }
}