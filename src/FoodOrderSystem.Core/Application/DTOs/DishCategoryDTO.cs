using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Domain.Model.Dish;
using FoodOrderSystem.Core.Domain.Model.DishCategory;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class DishCategoryDTO
    {
        public DishCategoryDTO(Guid id, string name, IEnumerable<DishDTO> dishes)
        {
            Id = id;
            Name = name;
            Dishes = new ReadOnlyCollection<DishDTO>(dishes.ToList());
        }

        internal DishCategoryDTO(DishCategory dishCategory, IEnumerable<Dish> dishes)
        {
            Id = dishCategory.Id.Value;
            Name = dishCategory.Name;
            Dishes = dishes.Select(en => new DishDTO(en)).ToList();
        }

        public Guid Id { get; }
        
        public string Name { get; }
        
        public IReadOnlyCollection<DishDTO> Dishes { get; }
    }
}
