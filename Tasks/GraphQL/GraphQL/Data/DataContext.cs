using GraphQL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Hero> Heroes { get; set; } = null!;

    public DbSet<Ability> Abilities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hero>().HasKey(x => x.Id);
        modelBuilder.Entity<Ability>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Hero>()
            .HasMany(x => x.Abilities)
            .WithMany(x => x.Heroes);
    }
}