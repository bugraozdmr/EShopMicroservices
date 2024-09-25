using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory;

// public record GetProductByCategoryRequest();
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                if (response.Products.Count() == 0)
                {
                    throw new Exception($"Category {category} not found");
                }
                return Results.Ok(response);
            })
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product by Category");
    }
}