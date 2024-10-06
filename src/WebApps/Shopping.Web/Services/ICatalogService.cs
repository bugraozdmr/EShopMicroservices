namespace Shopping.Web.Services;

public interface ICatalogService
{
    // postları kullanmadık hızlı olsun diye post delete vs için form gerek :/
    
    // Refit
    [Get("/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);
    
    // TODO : Muhtemelen id patlıcak patlarsa id:Guid dene
    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);
    
    [Get("/catalog-service/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductsByCategory(string category);
}