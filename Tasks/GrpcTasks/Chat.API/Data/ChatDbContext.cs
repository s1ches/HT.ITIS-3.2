using Chat.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext()
    {
    }

    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder) 
    {
        builder.UseSerialColumns();
    }
    
    public DbSet<Entities.Message> Messages { get; set; } = null!;
}