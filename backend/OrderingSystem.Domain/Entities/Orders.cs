using OrderingSystem.Domain.Constants;

namespace OrderingSystem.Domain.Entities;

public class Order
{
  private string _status = OrderStatus.PENDING;

  public Guid Id { get; set; }
  public string? Customer { get; set; }
  public string? Product { get; set; }
  public decimal Value { get; set; }
  public DateTime? CreatedAt { get; set; }
  public string? Status
  {
    get => _status;
    set
    {
      if (OrderStatus.IsValid(value ?? ""))
      {
        _status = value!;
      }
    }
  }
}
