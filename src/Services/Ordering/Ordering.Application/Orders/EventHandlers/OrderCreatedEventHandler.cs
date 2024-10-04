using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler
    (ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // when mediatr publish the domain event we will catch
        logger.LogInformation("Domain Event handled: {DomainEvent}",notification.GetType().Name);
        return Task.CompletedTask;
    }
}