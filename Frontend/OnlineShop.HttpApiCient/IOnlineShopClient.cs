using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.HttpApiCient
{
    public interface IOnlineShopClient
    {
        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task RemoveProduct(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
        Task RegistrateAccountAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken);
    }
}