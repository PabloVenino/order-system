
using System.Net;

namespace OrderingSystem.Application.Interfaces;

public interface IRateLimiterPolicy
{
  bool AllowRequestAsync(IPAddress ip);
}