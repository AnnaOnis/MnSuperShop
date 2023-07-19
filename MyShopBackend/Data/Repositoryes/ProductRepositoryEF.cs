

namespace MyShopBackend.Data.Repositoryes
{
    public class ProductRepositoryEF : EfRepository<Product> ,IProductRepository
    {
        public ProductRepositoryEF(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
