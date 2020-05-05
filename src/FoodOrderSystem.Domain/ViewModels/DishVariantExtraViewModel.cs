namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishVariantExtraViewModel
    {
        public DishVariantExtraViewModel(string name, string productInfo, decimal price)
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
