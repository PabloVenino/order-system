
namespace OrderingSystem.Domain.Constants;

public static class OrderStatus
{
  public const string PENDING = "PENDING";
  public const string PROCESSING = "PROCESSING";
  public const string COMPLETED = "COMPLETED";

  private static readonly HashSet<string> AllStatuses = [
    PENDING, PROCESSING, COMPLETED
  ];

  public static bool IsValid(string status)
  {
    if (!AllStatuses.Contains(status))
      throw new ArgumentException($"Invalid value for status: Status: {status}");
    else return true;
  }
}

