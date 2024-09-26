using Basket.API.Data;
using FluentValidation;

namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string Username) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(command => command.Username).NotEmpty().WithMessage("Username is required");
    }
}

public class DeleteBasketCommandHandler 
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        //todo delete basket from db and cache
        // session.Delete<Product>(command.id);

        await _basketRepository.DeleteBasket(command.Username,cancellationToken);
        
        return new DeleteBasketResult(true);
    }
}