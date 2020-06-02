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
            variants = new List<DishVariant>();
        }

        public Dish(DishId id, RestaurantId restaurantId, DishCategoryId categoryId, string name, string description,
            string productInfo, int orderNo, IList<DishVariant> variants)
            : this(id, restaurantId, categoryId)
        {
            Name = name;
            Description = description;
            ProductInfo = productInfo;
            OrderNo = orderNo;
            this.variants = variants;
        }

        public DishId Id { get; }
        public RestaurantId RestaurantId { get; }
        public DishCategoryId CategoryId { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ProductInfo { get; private set; }
        public int OrderNo { get; private set; }
        public IReadOnlyList<DishVariant> Variants => new ReadOnlyCollection<DishVariant>(variants);

        public Result<bool> ChangeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);
        
            Name = name;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(description));
            if (description.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(description), 100);

            Description = description;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeProductInfo(string productInfo)
        {
            if (productInfo.Length > 200)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(productInfo), 200);

            ProductInfo = productInfo;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeOrderNo(int orderNo)
        {
            if (orderNo < 0)
                return FailureResult<bool>.Create(FailureResultCode.DishInvalidOrderNo, nameof(orderNo));
            OrderNo = orderNo;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> AddVariant(Guid variantId, string name, decimal price)
        {
            return AddVariant(variants, variantId, name, price);
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
            if (variants == null)
            {
                this.variants = new List<DishVariant>();
                return SuccessResult<bool>.Create(true);
            }
            
            var tempVariants = new List<DishVariant>();

            foreach (var variant in variants)
            {
                var tempResult = AddVariant(tempVariants, variant.VariantId, variant.Name, variant.Price);
                if (tempResult.IsFailure)
                    return tempResult;
            }

            this.variants = tempVariants;
            return SuccessResult<bool>.Create(true);
        }

        private static Result<bool> AddVariant(IList<DishVariant> variants, Guid variantId, string name, decimal price)
        {
            if (variants == null)
                throw new ArgumentNullException(nameof(variants));
            
            if (variantId == Guid.Empty)
                throw new InvalidOperationException("variant has no id");
            if (variants.Any(en => en.VariantId == variantId))
                throw new InvalidOperationException("variant already exists");

            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 20)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 20);
            
            if (!(price > 0))
                return FailureResult<bool>.Create(FailureResultCode.DishVariantPriceIsNegativeOrZero);
            if (price > 200)
                return FailureResult<bool>.Create(FailureResultCode.DishVariantPriceIsTooBig);
            
            variants.Add(new DishVariant(variantId, name, price, new List<DishVariantExtra>()));
            return SuccessResult<bool>.Create(true);
        }
    }
}
