using System.Collections.Generic;
using System.Linq;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class DishCategoryBuilder : TestObjectBuilderBase<DishCategory>
    {
        public DishCategoryBuilder WithValidConstrains()
        {
            WithName("dish-category-name");
            WithOrderNo(1);
            WithConstrainedConstructorArgumentFor("dishes", () =>
            {
                var dishes = new List<Dish>();
                for (var dishIdx = 0; dishIdx < 3; dishIdx++)
                {
                    dishes.Add(new DishBuilder()
                        .WithName($"dish-name{dishIdx}")
                        .WithOrderNo(dishIdx)
                        .WithValidConstrains()
                        .Create()
                    );
                }

                return new Dishes(dishes);
            });
            return this;
        }

        public DishCategoryBuilder WithId(DishCategoryId id)
        {
            WithConstantConstructorArgumentFor("id", id);
            return this;
        }

        public DishCategoryBuilder WithName(string name)
        {
            WithConstantConstructorArgumentFor("name", name);
            return this;
        }

        public DishCategoryBuilder WithEnabled(bool enabled)
        {
            WithConstantConstructorArgumentFor("enabled", enabled);
            return this;
        }

        public DishCategoryBuilder WithOrderNo(int orderNo)
        {
            WithConstantConstructorArgumentFor("orderNo", orderNo);
            return this;
        }

        public DishCategoryBuilder WithoutDishes()
        {
            return WithDishes(Enumerable.Empty<Dish>());
        }

        public DishCategoryBuilder WithDishes(IEnumerable<Dish> dishes)
        {
            return WithDishes(new Dishes(dishes));
        }

        public DishCategoryBuilder WithDishes(Dishes dishes)
        {
            WithConstantConstructorArgumentFor("dishes", dishes);
            return this;
        }
    }
}
