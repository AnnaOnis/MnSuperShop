using MyShopFrontend.Models;

namespace MyShopFrontend.Services.Interfaces
{
    public interface ICatalog
    {
        void AddProduct(Product product);
        void ClearCatalog();
        Product GetProductById(Guid id);
        List<Product> GetProducts();
        void RemoveProduct(Guid id);
        void UpdateProduct(Product updatedProduct);
    }
}