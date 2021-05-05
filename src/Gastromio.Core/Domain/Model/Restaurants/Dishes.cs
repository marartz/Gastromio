using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class Dishes : IReadOnlyCollection<Dish>
    {
        private readonly IReadOnlyDictionary<DishId, Dish> dishDict;

        public Dishes(IEnumerable<Dish> dishes)
        {
            var tempDict = new Dictionary<DishId, Dish>();
            foreach (var dish in dishes)
            {
                if (tempDict.ContainsKey(dish.Id))
                    throw DomainException.CreateFrom(new DishAlreadyExistsFailure());
                if (tempDict.Any(en => string.Equals(en.Value.Name, dish.Name)))
                    throw DomainException.CreateFrom(new DishAlreadyExistsFailure());
                tempDict.Add(dish.Id, dish);
            }

            dishDict = new ReadOnlyDictionary<DishId, Dish>(tempDict);
        }

        public bool TryGetDish(DishId dishId, out Dish dish)
        {
            return dishDict.TryGetValue(dishId, out dish);
        }

        public Dishes AddOrChangeDish(DishId dishId, string name, string description, string productInfo, int orderNo,
            IEnumerable<DishVariant> variants, out Dish dish)
        {
            if (dishId == null)
            {
                dishId = new DishId(Guid.NewGuid());
            }

            dish = new Dish(
                dishId,
                name,
                description,
                productInfo,
                orderNo,
                new DishVariants(variants)
            );

            return Replace(dish);
        }

        public Dishes DecOrderOfDish(DishId dishId)
        {
            var orderedDishes = dishDict
                .OrderBy(en => en.Value.OrderNo)
                .Select(en => en.Value)
                .ToList();

            var pos = orderedDishes.FindIndex(en => en.Id == dishId);
            if (pos < 0)
                throw DomainException.CreateFrom(new DishDoesNotExistFailure());

            if (pos == 0)
                return this;

            var currentDish = orderedDishes[pos];
            var predecessorDish = orderedDishes[pos - 1];

            return new Dishes(
                dishDict
                    .Where(en => en.Value.Id != currentDish.Id && en.Value.Id != predecessorDish.Id)
                    .Select(en => en.Value)
                    .Append(predecessorDish.ChangeOrderNo(currentDish.OrderNo))
                    .Append(currentDish.ChangeOrderNo(predecessorDish.OrderNo))
            );
        }

        public Dishes IncOrderOfDish(DishId dishId)
        {
            var orderedDishes = dishDict
                .OrderBy(en => en.Value.OrderNo)
                .Select(en => en.Value)
                .ToList();

            var pos = orderedDishes.FindIndex(en => en.Id == dishId);
            if (pos < 0)
                throw DomainException.CreateFrom(new DishDoesNotExistFailure());

            if (pos >= dishDict.Count - 1)
                return this;

            var currentDish = orderedDishes[pos];
            var successorDish = orderedDishes[pos + 1];

            return new Dishes(
                dishDict
                    .Where(en => en.Value.Id != currentDish.Id && en.Value.Id != successorDish.Id)
                    .Select(en => en.Value)
                    .Append(successorDish.ChangeOrderNo(currentDish.OrderNo))
                    .Append(currentDish.ChangeOrderNo(successorDish.OrderNo))
            );
        }

        public Dishes AddDishVariant(DishId dishId, DishVariant dishVariant)
        {
            if (!TryGetDish(dishId, out var dish))
                throw DomainException.CreateFrom(new DishDoesNotExistFailure());
            return Replace(dish.AddDishVariant(dishVariant));
        }

        public Dishes RemoveDishVariant(DishId dishId, DishVariantId dishVariantId)
        {
            if (!TryGetDish(dishId, out var dish))
                throw DomainException.CreateFrom(new DishDoesNotExistFailure());
            return Replace(dish.RemoveDishVariant(dishVariantId));
        }

        public Dishes ReplaceDishVariants(DishId dishId, DishVariants dishVariants)
        {
            if (!TryGetDish(dishId, out var dish))
                throw DomainException.CreateFrom(new DishDoesNotExistFailure());
            return Replace(dish.ReplaceDishVariants(dishVariants));
        }

        public Dishes RemoveDish(DishId dishId)
        {
            return Remove(dishId);
        }

        private Dishes Replace(Dish dish)
        {
            var newDishes = dishDict
                .Where(en => en.Key != dish.Id)
                .Select(en => en.Value)
                .Append(dish);
            return new Dishes(newDishes);
        }

        private Dishes Remove(DishId dishId)
        {
            var newDishes = dishDict
                .Where(en => en.Key != dishId)
                .Select(en => en.Value);
            return new Dishes(newDishes);
        }

        public IEnumerator<Dish> GetEnumerator()
        {
            return dishDict
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
            get { return dishDict.Count; }
        }
    }
}
