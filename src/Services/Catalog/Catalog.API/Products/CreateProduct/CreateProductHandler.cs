using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

// diğer pattern daha iyi bu burda olmamalı şahsen
// degisim yaptik buildingblocksdan geliyor artik mediatr
public record CreateProductCommand(
    string Name,
    string Description,
    string ImageFile,
    List<string> Categories,
    decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

//internal kullandı ancak farklı dosyalara bolmek bence daha iyi
internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create product entity from command object
        // save database
        // return CreateProductResult result
        
        var product = new Product
        {
            Name = command.Name,
            Categories = command.Categories,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
        
        return new CreateProductResult(Guid.NewGuid());
    }
}