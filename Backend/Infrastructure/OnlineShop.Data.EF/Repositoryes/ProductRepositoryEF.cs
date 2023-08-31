using OnlineShop.Data.EF;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EF.Repositoryes
{
    public class ProductRepositoryEF : EfRepository<Product>, IProductRepository
    {
        public ProductRepositoryEF(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
