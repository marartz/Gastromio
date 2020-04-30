using FoodOrderSystem.Domain.Model.Cuisine;
using System;
using System.Text;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class CuisineViewModel
    {
        public CuisineViewModel(
            Guid id,
            string name
        )
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get;  }

        public string Name { get;  }

        public static CuisineViewModel FromCuisine(Cuisine cuisine)
        {
            return new CuisineViewModel(cuisine.Id.Value, cuisine.Name);
        }
    }
}
