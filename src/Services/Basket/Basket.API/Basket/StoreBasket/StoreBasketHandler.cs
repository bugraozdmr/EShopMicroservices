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

    public StoreBasketCommandHandler(IBasketRepository basketRepository,
        DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _basketRepository = basketRepository;
        _discountProtoService = discountProtoService;
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
        // communicate with discount.grpc
        foreach (var item in cart.Items)
        {
            var coupon = await _discountProtoService.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken:cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}