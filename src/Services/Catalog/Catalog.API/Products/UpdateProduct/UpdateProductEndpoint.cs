namespace Catalog.API.Products.UpdateProduct;

// for mapster -- bunları dto olarak gör aynı mantık çünkü
// Id product'daki ile eşleşmeli id olamaz
public record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    List<string> Categories,
    decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                // MAPSTER
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Product")
        .WithDescription("Update Product");
    }
}