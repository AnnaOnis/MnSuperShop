
using MyShop.HttpApiCient.Models;

public interface IMyShopClient
    {
        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task RemoveProduct(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
        Task RegistrateAccountAsync(Account account, CancellationToken cancellationToken);
    }
