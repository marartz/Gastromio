namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishVariantExtra
    {
        public DishVariantExtra(string name, string productInfo, decimal price)
        {
            Name = name;
            ProductInfo = productInfo;
            Price = price;
        }

        public string Name { get; }
        public string ProductInfo { get; }
        public decimal Price { get; }
    }
}
