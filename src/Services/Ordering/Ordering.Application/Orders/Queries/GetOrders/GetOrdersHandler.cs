using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersQuery,GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query,
        CancellationToken cancellationToken)
    {
        // get orders
        // return result

        var pageIndex = query.PaginationRequest.pageindex;
        var pageSize = query.PaginationRequest.pagesize;
        
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                pageIndex,
                pageSize,
                totalCount,
                orders.ToOrderDtoList()
            ));
    }
}