namespace Orders.Management.Requests.Order.CreateOrder;

public class CreateOrderRequest
{
    public required Guid ProductId { get; set; }
    
    public required int ProductsCount { get; set; }
    
    public required Guid CustomerId { get; set; }

    public required string Address { get; set; } = string.Empty;
}