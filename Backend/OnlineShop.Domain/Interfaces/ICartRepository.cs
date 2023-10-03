using OnlineShop.Domain.Entyties;

namespace OnlineShop.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByAccountId(Guid accountId, CancellationToken cancellationToken);       
    }
}