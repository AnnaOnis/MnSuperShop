namespace OnlineShop.Domain.Entyties
{
    public class CartItem : IEntity
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public double Quantity { get; set; }
        public Cart Cart { get; set; } = null!;
        public CartItem(Guid id, Guid productId, double quantity) 
        { 
            Id = id;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}