
namespace OrderingSystem.Application.Interfaces.Messaging;

public interface IServiceBusPublisher
{
  Task PublishAsync(object message);
}