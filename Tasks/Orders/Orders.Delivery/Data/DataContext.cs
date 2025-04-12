using Microsoft.EntityFrameworkCore;

namespace Orders.Delivery.Data;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<Entities.Delivery> Deliveries { get; set; }
}