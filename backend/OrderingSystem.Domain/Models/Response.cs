
namespace OrderingSystem.Domain.Models;

public record Response<T>
{
  public T? Data { get; set; }
  public Error? ErrorDetails { get; set; }
  public string? Message { get; set; }
}

public record Error
{
  public string? ErrorCode { get; set; }
  public bool IsError { get; set; }
}