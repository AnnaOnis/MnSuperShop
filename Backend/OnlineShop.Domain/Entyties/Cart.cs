using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entyties
{
    public class Cart : IEntity
    {
        public Guid Id { get; init; }
        public Guid AccountId { get; init; }

        public List<CartItem> CartItems { get; set; }
        public Cart(Guid id, Guid accountId) 
        {
            Id = id;
            AccountId = accountId;
            CartItems = new List<CartItem>();
        }

        public Task AddItem(Guid productId, double quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (CartItems == null) throw new InvalidOperationException(message: "Cart items is null");

            var existedCartItem = CartItems.SingleOrDefault(item => item.ProductId == productId);
            if (existedCartItem is null)
            {
                CartItems.Add(new CartItem(Guid.Empty, productId, quantity));
            }
            else
            {
                existedCartItem.Quantity += quantity;
            }
            return Task.CompletedTask;
        }

    }
}
