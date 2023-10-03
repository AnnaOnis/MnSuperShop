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

        public List<CartItem>? CartItems { get; set; }

    }
}
