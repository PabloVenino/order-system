
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using OrderingSystem.Application.Interfaces;

namespace OrderingSystem.Infra.Middlewares;

public class RateLimitingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly IRateLimiterPolicy _policy;

  public RateLimitingMiddleware(RequestDelegate next, IRateLimiterPolicy policy)
  {
    _next = next;
    _policy = policy;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    IPAddress clientIp = context.Connection.RemoteIpAddress;

    if (!_policy.AllowRequestAsync(clientIp))
    {
      context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
      return;
    }

    await _next(context);
  }
}