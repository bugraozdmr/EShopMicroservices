using BuildingBlocks.Exceptions;

namespace Ordering.Application.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    // buildingblocks'dan geldi
    public OrderNotFoundException(Guid id) : base("Order",id)
    {
    }
}