using FoodOrderSystem.Domain.Model.Cuisine;
using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class CuisineViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public static CuisineViewModel FromCuisine(Cuisine cuisine)
        {
            return new CuisineViewModel
            {
                Id = cuisine.Id.Value,
                Name = cuisine.Name
            };
        }
    }
}
