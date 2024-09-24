using MediatR;

namespace BuildingBlocks.CQRS;

// doesnt return response
public interface ICommand : IRequest<Unit>
{
    
}
// returns response
public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}