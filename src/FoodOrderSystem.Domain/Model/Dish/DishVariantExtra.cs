using System;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishVariantExtra
    {
        public DishVariantExtra(Guid extraId, string name, string productInfo, decimal price)
        {
            ExtraId = extraId;
            Name = name;
            ProductInfo = productInfo;
            Price = price;
        }

        public Guid ExtraId { get; }
        public string Name { get; private set; }
        public string ProductInfo { get; private set; }
        public decimal Price { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangeProductInfo(string productInfo)
        {
            ProductInfo = productInfo;
        }

        public void ChangePrice(decimal price)
        {
            Price = price;
        }
    }
}
