using System;
using System.Collections.Generic;
using System.Linq;
using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.DTOs
{
    public class DishDTO
    {
        internal DishDTO(Dish dish)
        {
            Id = dish.Id.Value;
            Name = dish.Name;
            Description = dish.Description;
            ProductInfo = dish.ProductInfo;
            OrderNo = dish.OrderNo;
            Variants = dish.Variants.Select(en => new DishVariantDTO(en)).ToList();
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string ProductInfo { get; }

        public int OrderNo { get; }

        public IReadOnlyCollection<DishVariantDTO> Variants { get; }
    }
}
