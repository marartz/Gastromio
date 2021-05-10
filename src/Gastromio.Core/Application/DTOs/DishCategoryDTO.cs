using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.DTOs
{
    public class DishCategoryDTO
    {
        internal DishCategoryDTO(DishCategory dishCategory)
        {
            Id = dishCategory.Id.Value;
            Name = dishCategory.Name;
            Enabled = dishCategory.Enabled;
            Dishes = dishCategory.Dishes.Select(en => new DishDTO(en)).ToList();
        }

        public Guid Id { get; }

        public string Name { get; }

        public bool Enabled { get; }

        public IReadOnlyCollection<DishDTO> Dishes { get; }
    }
}
