using Microsoft.EntityFrameworkCore;
using Orders.Common.Interceptors;
using Orders.Common.Messages;
using Orders.Management.Data.Entities;

namespace Orders.Management.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderCreatedMessage> OrderCreatedMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new DataContextSaveChangesInterceptor());
        base.OnConfiguring(optionsBuilder);
    }
}