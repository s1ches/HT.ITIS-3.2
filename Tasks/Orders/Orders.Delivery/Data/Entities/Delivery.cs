using Orders.Delivery.Data.Enums;

namespace Orders.Delivery.Data.Entities;

public class Delivery
{
    public Guid Id { get; set; }
    
    public string Address { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public Guid ProductId { get; set; }
    
    public Guid OrderId { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public DateTime UpdateDate { get; set; }
    
    public DeliveryState DeliveryState { get; set; }
    
    public int ProductsCount { get; set; }
}