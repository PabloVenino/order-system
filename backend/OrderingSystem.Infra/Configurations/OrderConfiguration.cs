
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderingSystem.Domain.Entities;
using OrderingSystem.Domain.Constants;

namespace OrderingSystem.Infra.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> builder)
  {
    builder.ToTable("orders");

    builder.HasKey(o => o.Id);
    
    builder.Property(o => o.Customer)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(o => o.Product)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(o => o.Value)
      .HasPrecision(18, 2)
      .IsRequired();

    builder.Property(o => o.Status)
      .HasDefaultValue(OrderStatus.PENDING)
      .HasMaxLength(20)
      .IsRequired();

    builder.Property(o => o.CreatedAt)
      .HasDefaultValueSql("NOW()")
      .IsRequired();
  }
}