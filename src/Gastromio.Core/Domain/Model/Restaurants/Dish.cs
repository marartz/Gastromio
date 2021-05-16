using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class Dish
    {
        public Dish(
            DishId id,
            string name,
            string description,
            string productInfo,
            int orderNo,
            DishVariants variants
        )
        {
            ValidateName(name);
            ValidateDescription(description);
            ValidateProductInfo(productInfo);
            ValidateOrderNo(orderNo);

            Id = id;
            Name = name;
            Description = description;
            ProductInfo = productInfo;
            OrderNo = orderNo;
            Variants = variants;
        }

        public DishId Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string ProductInfo { get; }

        public int OrderNo { get; }

        public DishVariants Variants { get; }

        public Dish ChangeName(string name)
        {
            return new Dish(
                Id,
                name,
                Description,
                ProductInfo,
                OrderNo,
                Variants
            );
        }

        public Dish ChangeDescription(string description)
        {
            return new Dish(
                Id,
                Name,
                description,
                ProductInfo,
                OrderNo,
                Variants
            );
        }

        public Dish ChangeProductInfo(string productInfo)
        {
            return new Dish(
                Id,
                Name,
                Description,
                productInfo,
                OrderNo,
                Variants
            );
        }

        public Dish ChangeOrderNo(int orderNo)
        {
            return new Dish(
                Id,
                Name,
                Description,
                ProductInfo,
                orderNo,
                Variants
            );
        }

        public Dish AddDishVariant(DishVariant dishVariant)
        {
            return new Dish(
                Id,
                Name,
                Description,
                ProductInfo,
                OrderNo,
                Variants.AddDishVariant(dishVariant)
            );
        }

        public Dish RemoveDishVariant(DishVariantId dishVariantId)
        {
            return new Dish(
                Id,
                Name,
                Description,
                ProductInfo,
                OrderNo,
                Variants.RemoveDishVariant(dishVariantId)
            );
        }

        public Dish ReplaceDishVariants(DishVariants dishVariants)
        {
            return new Dish(
                Id,
                Name,
                Description,
                ProductInfo,
                OrderNo,
                dishVariants
            );
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw DomainException.CreateFrom(new DishNameRequiredFailure());
            if (name.Length > 40)
                throw DomainException.CreateFrom(new DishNameTooLongFailure(40));
        }

        private static void ValidateDescription(string description)
        {
            if (description != null && description.Length > 200)
                throw DomainException.CreateFrom(new DishDescriptionTooLongFailure(200));
        }

        private static void ValidateProductInfo(string productInfo)
        {
            if (productInfo != null && productInfo.Length > 200)
                throw DomainException.CreateFrom(new DishProductInfoTooLongFailure(200));
        }

        private static void ValidateOrderNo(int orderNo)
        {
            if (orderNo < 0)
                throw DomainException.CreateFrom(new DishInvalidOrderNoFailure());
        }
    }
}
