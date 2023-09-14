using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.HttpApiCient
{
    public interface IOnlineShopClient
    {
        bool IsAuthorizationTokenSet { get; set; }

        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task RemoveProduct(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
        Task<RegisterResponse> RegistrateAccountAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken);
        Task<AccountResponse> GetCurrentAccount(CancellationToken cancellationToken);
        void SetAuthorizationToken(string token);
        void ResetAuthorizationToken();
    }
}