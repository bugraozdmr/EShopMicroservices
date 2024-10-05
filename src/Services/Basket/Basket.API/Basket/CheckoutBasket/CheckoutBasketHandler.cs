using Basket.API.Data;
using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) 
    : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can not be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("Username is required");
    }
}

// IPublishEndpoint -- masstransit
public class CheckoutBasketCommandHandler
    (IBasketRepository repository,IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket
        var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName,cancellationToken);
        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }
        
        // get totalprice on basketcheckout event message
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        
        // send basket checkout to rabbitmq using masstransit
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        
        // delete the basket
        await repository.DeleteBasket(command.BasketCheckoutDto.UserName,cancellationToken);

        return new CheckoutBasketResult(true);
    }
}