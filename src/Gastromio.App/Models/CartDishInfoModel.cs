using System;

namespace Gastromio.App.Models
{
    public class CartDishInfoModel
    {
        public Guid ItemId { get; set; }
        public Guid DishId { get; set; }
        public Guid VariantId { get; set; }
        public int Count { get; set; }
        public string Remarks { get; set; }

    }
}