using OnlineShop.Domain.Entyties;
using System.Xml.Linq;

namespace MySyperShop.Domain.Tests
{
    public class CartTests
    {
        [Fact]
        public void Item_is_added_to_cart()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            //Act
            cart.AddItem(productId, 1d);
            
            //Assert
            Assert.Single(cart.CartItems!);
        }

        [Fact]
        public void Adding_an_existing_item_to_cart_increases_its_quantity()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            //Act
            cart.AddItem(productId, 1d);
            cart.AddItem(productId, 1d);

            //Assert
            Assert.Single(cart.CartItems!);
            Assert.Equal(2d, cart.CartItems.First(it=>it.ProductId == productId).Quantity);
        }

        [Fact]
        public void When_creating_the_cart_it_is_empty()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());

            //Assert
            Assert.Empty(cart.CartItems);
        }

        [Fact]
        public void Different_items_is_added_to_cart()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productOneId = Guid.NewGuid();
            var productTwoId = Guid.NewGuid();

            //Act
            cart.AddItem(productOneId, 1d);
            cart.AddItem(productTwoId, 1d);

            //Assert
            Assert.Equal(2d, cart.CartItems.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Item_with_an_invalid_quantity_causes_exception(int quantity)
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cart.AddItem(productId, quantity));
        }


    }
}