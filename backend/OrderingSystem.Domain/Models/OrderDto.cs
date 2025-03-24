
namespace OrderingSystem.Domain.Models;

public class OrderDto
{
  public string? Customer { get; set; }
  public string? Product { get; set; }
  public decimal Value { get; set; }
}
