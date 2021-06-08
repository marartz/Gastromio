using System;
using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.DTOs
{
    public class CartDishInfoDTO
    {
        public CartDishInfoDTO(
            Guid itemId,
            DishId dishId,
            DishVariantId variantId,
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
        public DishVariantId VariantId { get; }
        public int Count { get; }
        public string Remarks { get; }
    }
}
