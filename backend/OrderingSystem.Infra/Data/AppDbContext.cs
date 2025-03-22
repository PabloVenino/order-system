using Microsoft.EntityFrameworkCore;
using OrderingSystem.Domain.Entities;

namespace OrderingSystem.Infra.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Order> Orders { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}