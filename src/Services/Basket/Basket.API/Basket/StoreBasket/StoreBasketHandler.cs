using Basket.API.Data;
using Basket.API.Models;
using Discount.Grpc;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string Username);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotEmpty().WithMessage("Cart can not be empty");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
    }
}

public class StoreBasketCommandHandler 
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;
    private readonly ILogger<StoreBasketCommandHandler> _logger;

    public StoreBasketCommandHandler(IBasketRepository basketRepository,
        DiscountProtoService.DiscountProtoServiceClient discountProtoService,
        ILogger<StoreBasketCommandHandler> logger)
    {
        _basketRepository = basketRepository;
        _discountProtoService = discountProtoService;
        _logger = logger;
    }

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart,cancellationToken);
        
        // store basket in db (use marten)
        await _basketRepository.StoreBasket(command.Cart, cancellationToken);
        
        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        _logger.LogInformation("*** Deducting discount inside ***");
        // communicate with discount.grpc
        foreach (var item in cart.Items)
        {
            _logger.LogInformation("--- Processing discount item ---");
            var coupon = await _discountProtoService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken:cancellationToken);
            item.Price -= coupon.Amount;
        }
        _logger.LogInformation("*** Deducting discount out ***");
    }
}