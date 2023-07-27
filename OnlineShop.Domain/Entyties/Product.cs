namespace OnlineShop.Domain.Entyties
{
    public class Product : IEntity
    {
        /// <summary> ID товара </summary>
        public Guid Id { get; init; }
        /// <summary> Изображение товара </summary>
        public string? ImageUrl { get; set; }
        /// <summary> Название товара </summary>
        public string? Name { get; set; }

        /// <summary> Описание товара </summary>
        public string? Description { get; set; }

        /// <summary> Цена товара </summary>
        public decimal Price { get; set; }

        /// <summary> Информация о скидках </summary>
        public string? DescriptionDiscount { get; set; }

        /// <summary> Цена товара со скидкой</summary>
        public decimal DiscountPrice { get; set; }

        /// <summary> Дата изготовления </summary>
        public DateTime ProducedAt { get; set; }

        /// <summary> Срок годности </summary>
        public DateTime ExpiredAt { get; set; }

        /// <summary> Количество товара в наличии </summary>
        public double Stock { get; set; }
    }
}