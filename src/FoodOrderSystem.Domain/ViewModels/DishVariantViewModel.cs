using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishVariantViewModel
    {
        public DishVariantViewModel(string name, decimal price, IList<DishVariantExtraViewModel> extras)
        {
            Name = name;
            Price = price;
            Extras = new ReadOnlyCollection<DishVariantExtraViewModel>(extras);
        }

        public string Name { get; }
        public decimal Price { get; }
        public IReadOnlyList<DishVariantExtraViewModel> Extras { get; }
    }
}
