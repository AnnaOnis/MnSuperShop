using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Entyties;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork uow)
        {
            _unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public virtual async Task AddProduct(Guid accountId, Product product, double quantity, CancellationToken cancellationToken)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));
            if(quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            var cart = await _unitOfWork.CartRepository.GetCartByAccountId(accountId, cancellationToken);
            var existedCartItem = cart.CartItems.FirstOrDefault(item => item.ProductId ==  product.Id);
            if (existedCartItem is null)
            {
                cart.CartItems.Add(new CartItem ( Guid.Empty, product.Id, quantity));
            }
            else
            {
                existedCartItem.Quantity += quantity;
            }

            await _unitOfWork.CartRepository.Update(cart, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
        {
            return _unitOfWork.CartRepository.GetCartByAccountId(accountId, cancellationToken);
        }
    }
}
