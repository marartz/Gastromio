using System;
using FoodOrderSystem.Domain.Model.Dish;

namespace FoodOrderSystem.Domain.Commands.Checkout
{
    public class OrderedDishInfo
    {
        public OrderedDishInfo(
            Guid itemId,
            DishId dishId,
            Guid variantId,
            int count,
            string remarks
        )
        {
            ItemId = itemId;
            DishId = dishId;
            VariantId = variantId;
            Count = count;
            Remarks = remarks;
        }
        
        public Guid ItemId { get; }
        public DishId DishId { get; }
        public Guid VariantId { get; }
        public int Count { get; }
        public string Remarks { get; }
    }
}