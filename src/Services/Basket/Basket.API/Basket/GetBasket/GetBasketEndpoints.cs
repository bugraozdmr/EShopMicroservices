using Basket.API.Models;


namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string Username);
public record GetBasketResponse(ShoppingCart Cart);

// Bunu yuklemek zorundasın bu assemly kabul etmiyor
public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));
            var response = result.Adapt<GetBasketResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasketEndpoints")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("GetBasket")
        .WithDescription("GetBasket");
    }
}