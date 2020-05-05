using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishViewModel
    {
        public DishViewModel(Guid id, string name, string description, string productInfo, IList<DishVariantViewModel> variants)
        {
            Id = id;
            Name = name;
            Description = description;
            ProductInfo = productInfo;
            Variants = new ReadOnlyCollection<DishVariantViewModel>(variants);
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string ProductInfo { get; }
        public IReadOnlyList<DishVariantViewModel> Variants { get; }
    }
}
