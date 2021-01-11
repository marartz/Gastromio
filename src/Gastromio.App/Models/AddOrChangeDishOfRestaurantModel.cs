using System;
using Gastromio.Core.Application.DTOs;

namespace Gastromio.App.Models
{
    public class AddOrChangeDishOfRestaurantModel
    {
        public Guid DishCategoryId { get; set; }
        public DishModel Dish { get; set; }
    }
}
