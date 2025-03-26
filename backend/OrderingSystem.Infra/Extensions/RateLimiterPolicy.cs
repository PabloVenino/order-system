
using System.Collections.Concurrent;
using System.Net;
using System.Security.Principal;
using Microsoft.Extensions.Azure;
using OrderingSystem.Application.Interfaces;

namespace OrderingSystem.Infra.Extensions;

public class RateLimiterPolicy(int limit, TimeSpan window) : IRateLimiterPolicy
{
  private readonly int _limit = limit;
  private readonly TimeSpan _window = window;
  private readonly ConcurrentDictionary<IPAddress, (int count, DateTime lastRequest)> _clientRequests = new();

  public bool AllowRequestAsync(IPAddress ip)
  {
    if (!_clientRequests.ContainsKey(ip))
    {
      _clientRequests.TryAdd(ip, (1, DateTime.UtcNow));
      return true;
    }

    var (count, lastRequest) = _clientRequests[ip];

    if (DateTime.UtcNow - lastRequest > window)
    {
      UpdateRequestsDictionary(ip, count);
      return true;
    }

    if (count < _limit)
    {
      UpdateRequestsDictionary(ip, count);
      return true;
    }

    return false;
  }

  private void UpdateRequestsDictionary(IPAddress ip, int count)
  {
    _clientRequests[ip] = (count++, DateTime.UtcNow);
  }
}