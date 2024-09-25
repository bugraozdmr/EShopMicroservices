namespace Catalog.API.Products.DeleteProduct;

// dökümandaki aynı mantık ile oluşturur isimli recordları -- bu get oldugu icin bir sey yollanmiyor ondan requeste gerek yok
//public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            // id yoksada silmiş gibi ok dönecektir bilgi vermeme adına iyi bir şey bence
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}