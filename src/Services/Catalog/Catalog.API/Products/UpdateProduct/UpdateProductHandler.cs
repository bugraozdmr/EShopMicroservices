using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    List<string> Categories,
    decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandHandler 
    (IDocumentSession session,ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Query}",command);
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Categories = command.Categories;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(true);
    }
}