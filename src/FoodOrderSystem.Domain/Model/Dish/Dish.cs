using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class Dish
    {
        private IList<DishVariant> variants;

        public Dish(DishId id, RestaurantId restaurantId, DishCategoryId categoryId)
        {
            Id = id;
            RestaurantId = restaurantId;
            CategoryId = categoryId;
        }

        public Dish(DishId id, RestaurantId restaurantId, DishCategoryId categoryId, string name, string description,
            string productInfo, IList<DishVariant> variants)
            : this(id, restaurantId, categoryId)
        {
            Name = name;
            Description = description;
            ProductInfo = productInfo;
            this.variants = variants;
        }

        public DishId Id { get; }
        public RestaurantId RestaurantId { get; }
        public DishCategoryId CategoryId { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ProductInfo { get; private set; }
        public IReadOnlyList<DishVariant> Variants => new ReadOnlyCollection<DishVariant>(variants);

        public Result<bool> ChangeName(string name)
        {
            Name = name;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeDescription(string description)
        {
            Description = description;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeProductInfo(string productInfo)
        {
            ProductInfo = productInfo;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddVariant(Guid variantId, string name, decimal price)
        {
            if (variants.Any(en => en.VariantId == variantId))
                throw new InvalidOperationException("variant already exists");
            variants.Add(new DishVariant(variantId, name, price, new List<DishVariantExtra>()));
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RemoveVariant(Guid variantId)
        {
            var variant = variants.FirstOrDefault(en => en.VariantId == variantId);
            if (variant == null)
                throw new InvalidOperationException("variant does not exist");
            variants.Remove(variant);
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ReplaceVariants(IList<DishVariant> variants)
        {
            this.variants = variants;
            return SuccessResult<bool>.Create(true);
        }
    }
}
