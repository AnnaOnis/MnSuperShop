using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Interfaces;
using OnlineShop.WebApi.Data;

namespace OnlineShop.WebApi.Data.Repositoryes
{
    public class ProductRepositoryEF : EfRepository<Product>, IProductRepository
    {
        public ProductRepositoryEF(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
