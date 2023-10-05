using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.EF.Repositoryes
{
    public class CartRepositoryEF : EfRepository<Cart>, ICartRepository
    {
        public CartRepositoryEF(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Cart> GetCartByAccountId(Guid accountId, CancellationToken cancellationToken)
        {
            return Entities.Include(cart => cart.CartItems)
                           .SingleAsync(e => e.AccountId == accountId, cancellationToken);
        }
    }
}
