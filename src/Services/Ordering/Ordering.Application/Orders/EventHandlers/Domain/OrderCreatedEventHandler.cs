using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler
    (IPublishEndpoint publishEndpoint,
        IFeatureManager featureManager,
        ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // when mediatr publish the domain event we will catch
        logger.LogInformation("Domain Event handled: {DomainEvent}",domainEvent.GetType().Name);

        if (await featureManager.IsEnabledAsync("OrderFulfilment"))
        {
            // eger dogruysa gelcek global flag işte
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
            // orderDto ilk parametre :D
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);    
        }
    }
}