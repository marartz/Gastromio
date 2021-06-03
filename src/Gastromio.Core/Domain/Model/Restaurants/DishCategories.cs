using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishCategories : IEnumerable<DishCategory>
    {
        private readonly IReadOnlyDictionary<DishCategoryId, DishCategory> dishCategoryDict;

        public DishCategories(IEnumerable<DishCategory> dishCategories)
        {
            var tempDict = new Dictionary<DishCategoryId, DishCategory>();
            foreach (var dishCategory in dishCategories)
            {
                if (tempDict.ContainsKey(dishCategory.Id))
                    throw DomainException.CreateFrom(new DishCategoryAlreadyExistsFailure());
                if (tempDict.Any(en => string.Equals(en.Value.Name, dishCategory.Name)))
                    throw DomainException.CreateFrom(new DishCategoryAlreadyExistsFailure());
                tempDict.Add(dishCategory.Id, dishCategory);
            }

            dishCategoryDict = new ReadOnlyDictionary<DishCategoryId, DishCategory>(tempDict);
        }

        public bool TryGetDishCategory(DishCategoryId dishCategoryId, out DishCategory dishCategory)
        {
            return dishCategoryDict.TryGetValue(dishCategoryId, out dishCategory);
        }

        public bool TryGetDish(DishId dishId, out DishCategory dishCategory, out Dish dish)
        {
            foreach (var dishCategoryEn in dishCategoryDict)
            {
                if (dishCategoryEn.Value.Dishes.TryGetDish(dishId, out dish))
                {
                    dishCategory = dishCategoryEn.Value;
                    return true;
                }
            }

            dishCategory = null;
            dish = null;
            return false;
        }

        public DishCategories AddNewDishCategory(string name, DishCategoryId afterDishCategoryId,
            out DishCategory dishCategory)
        {
            if (dishCategoryDict.Any(en => string.Equals(en.Value.Name, name)))
                throw DomainException.CreateFrom(new DishCategoryAlreadyExistsFailure());

            var orderedCategoryList = dishCategoryDict
                .Select(en => en.Value)
                .OrderBy(en => en.OrderNo)
                .ToList();

            var pos = orderedCategoryList.FindIndex(en => en.Id == afterDishCategoryId);
            if (pos < -1)
                pos = orderedCategoryList.Count - 1;

            var newDishCategories = new List<DishCategory>();

            for (var i = 0; i <= pos; i++)
            {
                var curDishCategory = orderedCategoryList[i];
                newDishCategories.Add(new DishCategory(
                    curDishCategory.Id,
                    curDishCategory.Name,
                    i,
                    curDishCategory.Enabled,
                    curDishCategory.Dishes
                ));
            }

            dishCategory = new DishCategory(
                new DishCategoryId(Guid.NewGuid()),
                name,
                pos + 1,
                true,
                new Dishes(Enumerable.Empty<Dish>())
            );
            newDishCategories.Add(dishCategory);

            for (var i = pos + 1; i < orderedCategoryList.Count; i++)
            {
                var curDishCategory = orderedCategoryList[i];
                newDishCategories.Add(new DishCategory(
                    curDishCategory.Id,
                    curDishCategory.Name,
                    i + 1,
                    curDishCategory.Enabled,
                    curDishCategory.Dishes
                ));
            }

            return new DishCategories(newDishCategories);
        }

        public DishCategories RemoveDishCategory(DishCategoryId dishCategoryId)
        {
            return TryGetDishCategory(dishCategoryId, out _)
                ? Remove(dishCategoryId)
                : this;
        }

        public DishCategories ChangeDishCategoryName(DishCategoryId dishCategoryId, string name)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.ChangeName(name));
        }

        public DishCategories EnableDishCategory(DishCategoryId dishCategoryId)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.Enable());
        }

        public DishCategories DisableDishCategory(DishCategoryId dishCategoryId)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.Disable());
        }

        public DishCategories DecOrderOfDishCategory(DishCategoryId dishCategoryId)
        {
            var orderedDishCategories = dishCategoryDict
                .OrderBy(en => en.Value.OrderNo)
                .Select(en => en.Value)
                .ToList();

            var pos = orderedDishCategories.FindIndex(en => en.Id == dishCategoryId);
            if (pos < 0)
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());

            if (pos == 0)
                return this;

            var currentDishCategory = orderedDishCategories[pos];
            var predecessorDishCategory = orderedDishCategories[pos - 1];

            return new DishCategories(
                dishCategoryDict
                    .Where(en => en.Value.Id != currentDishCategory.Id && en.Value.Id != predecessorDishCategory.Id)
                    .Select(en => en.Value)
                    .Append(predecessorDishCategory.ChangeOrderNo(currentDishCategory.OrderNo))
                    .Append(currentDishCategory.ChangeOrderNo(predecessorDishCategory.OrderNo))
            );
        }

        public DishCategories IncOrderOfDishCategory(DishCategoryId dishCategoryId)
        {
            var orderedDishCategories = dishCategoryDict
                .OrderBy(en => en.Value.OrderNo)
                .Select(en => en.Value)
                .ToList();

            var pos = orderedDishCategories.FindIndex(en => en.Id == dishCategoryId);
            if (pos < 0)
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());

            if (pos >= dishCategoryDict.Count - 1)
                return this;

            var currentDishCategory = orderedDishCategories[pos];
            var successorDishCategory = orderedDishCategories[pos + 1];

            return new DishCategories(
                dishCategoryDict
                    .Where(en => en.Value.Id != currentDishCategory.Id && en.Value.Id != successorDishCategory.Id)
                    .Select(en => en.Value)
                    .Append(successorDishCategory.ChangeOrderNo(currentDishCategory.OrderNo))
                    .Append(currentDishCategory.ChangeOrderNo(successorDishCategory.OrderNo))
            );
        }

        public DishCategories AddOrChangeDish(DishCategoryId dishCategoryId, DishId dishId, string name,
            string description, string productInfo, int orderNo, IEnumerable<DishVariant> variants, out Dish dish)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.AddOrChangeDish(dishId, name, description, productInfo, orderNo, variants,
                out dish));
        }

        public DishCategories DecOrderOfDish(DishCategoryId dishCategoryId, DishId dishId)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.DecOrderOfDish(dishId));
        }

        public DishCategories IncOrderOfDish(DishCategoryId dishCategoryId, DishId dishId)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.IncOrderOfDish(dishId));
        }

        public DishCategories AddDishVariant(DishCategoryId dishCategoryId, DishId dishId, DishVariant dishVariant)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.AddDishVariant(dishId, dishVariant));
        }

        public DishCategories RemoveDishVariant(DishCategoryId dishCategoryId, DishId dishId,
            DishVariantId dishVariantId)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.RemoveDishVariant(dishId, dishVariantId));
        }

        public DishCategories ReplaceDishVariants(DishCategoryId dishCategoryId, DishId dishId,
            DishVariants dishVariants)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.ReplaceDishVariants(dishId, dishVariants));
        }

        public DishCategories RemoveDish(DishCategoryId dishCategoryId, DishId dishId)
        {
            if (!TryGetDishCategory(dishCategoryId, out var dishCategory))
                throw DomainException.CreateFrom(new DishCategoryDoesNotExistFailure());
            return Replace(dishCategory.RemoveDish(dishId));
        }

        public IEnumerator<DishCategory> GetEnumerator()
        {
            return dishCategoryDict
                .OrderBy(en => en.Value.OrderNo)
                .Select(en => en.Value)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return dishCategoryDict.Count; }
        }

        private DishCategories Replace(DishCategory dishCategory)
        {
            var newDishCategories = dishCategoryDict
                .Where(en => en.Key != dishCategory.Id)
                .Select(en => en.Value)
                .Append(dishCategory);
            return new DishCategories(newDishCategories);
        }

        private DishCategories Remove(DishCategoryId dishCategoryId)
        {
            var newDishCategories = dishCategoryDict
                .Where(en => en.Key != dishCategoryId)
                .Select(en => en.Value);
            return new DishCategories(newDishCategories);
        }
    }
}
