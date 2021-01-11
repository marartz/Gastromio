using System.Collections.Generic;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.DishCategory;
using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Dish
{
    public interface IDishFactory
    {
        Result<Dish> Create(
            RestaurantId restaurantId,
            DishCategoryId categoryId,
            string name,
            string description,
            string productInfo,
            int orderNo,
            IEnumerable<DishVariant> variants,
            UserId createdBy
        );
    }
}