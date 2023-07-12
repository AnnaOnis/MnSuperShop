using MyShopFrontend.Models;

namespace MyShopFrontend.Services.Interfaces
{
    public interface IMyShopClient
    {
        Task<Product[]> GetProducts();
        Task<Product> GetProduct(Guid id);
        Task AddProduct(Product product);
        Task RemoveProduct(Guid id);
        Task UpdateProduct(Product product);
    }
}