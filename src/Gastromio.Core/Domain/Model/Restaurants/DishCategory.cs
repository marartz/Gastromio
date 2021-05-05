using System.Collections.Generic;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishCategory
    {
        public DishCategory(
            DishCategoryId id,
            string name,
            int orderNo,
            bool enabled,
            Dishes dishes
        )
        {
            ValidateName(name);
            ValidateOrderNo(orderNo);

            Id = id;
            Name = name;
            OrderNo = orderNo;
            Enabled = enabled;
            Dishes = dishes;
        }

        public DishCategoryId Id { get; }

        public string Name { get; }

        public int OrderNo { get; }

        public bool Enabled { get; }

        public Dishes Dishes { get; }

        public DishCategory ChangeName(string name)
        {
            return new DishCategory(
                Id,
                name,
                OrderNo,
                Enabled,
                Dishes
            );
        }

        public DishCategory ChangeOrderNo(int orderNo)
        {
            return new DishCategory(
                Id,
                Name,
                orderNo,
                Enabled,
                Dishes
            );
        }

        public DishCategory Enable()
        {
            if (Enabled)
                return this;
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                true,
                Dishes
            );
        }

        public DishCategory Disable()
        {
            if (!Enabled)
                return this;
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                false,
                Dishes
            );
        }

        public DishCategory AddOrChangeDish(DishId dishId, string name, string description, string productInfo,
            int orderNo, IEnumerable<DishVariant> variants, out Dish dish)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.AddOrChangeDish(dishId, name, description, productInfo, orderNo, variants, out dish)
            );
        }

        public DishCategory DecOrderOfDish(DishId dishId)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.DecOrderOfDish(dishId)
            );
        }

        public DishCategory IncOrderOfDish(DishId dishId)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.IncOrderOfDish(dishId)
            );
        }

        public DishCategory AddDishVariant(DishId dishId, DishVariant dishVariant)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.AddDishVariant(dishId, dishVariant)
            );
        }

        public DishCategory RemoveDishVariant(DishId dishId, DishVariantId dishVariantId)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.RemoveDishVariant(dishId, dishVariantId)
            );
        }

        public DishCategory ReplaceDishVariants(DishId dishId, DishVariants dishVariants)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.ReplaceDishVariants(dishId, dishVariants)
            );
        }

        public DishCategory RemoveDish(DishId dishId)
        {
            return new DishCategory(
                Id,
                Name,
                OrderNo,
                Enabled,
                Dishes.RemoveDish(dishId)
            );
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw DomainException.CreateFrom(new DishCategoryNameRequiredFailure());
            if (name.Length > 100)
                throw DomainException.CreateFrom(new DishCategoryNameTooLongFailure());
        }

        private static void ValidateOrderNo(int orderNo)
        {
            if (orderNo < 0)
                throw DomainException.CreateFrom(new DishCategoryInvalidOrderNoFailure());
        }
    }
}
