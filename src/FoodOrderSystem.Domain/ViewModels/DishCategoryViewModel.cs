using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishCategoryViewModel
    {
        public DishCategoryViewModel(Guid id, string name, IList<DishViewModel> dishes)
        {
            Id = id;
            Name = name;
            Dishes = new ReadOnlyCollection<DishViewModel>(dishes);
        }

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyList<DishViewModel> Dishes { get; }
    }
}
