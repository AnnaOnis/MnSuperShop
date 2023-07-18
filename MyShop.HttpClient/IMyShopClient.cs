using System;
using System.Threading;
using System.Threading.Tasks;


    public interface IMyShopClient
    {
        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task RemoveProduct(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, CancellationToken cancellationToken);
    }
