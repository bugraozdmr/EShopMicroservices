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
// marten direkt inject edilir zaten abstract
internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
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
        
        // marten oto tabloyuda oluşturcak -- mt_doc_product --
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new CreateProductResult(product.Id);
    }
}