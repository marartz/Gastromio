using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.DishCategory
{
    public interface IDishCategoryFactory
    {
        Result<DishCategory> Create(
            RestaurantId restaurantId,
            string name,
            int orderNo,
            UserId createdBy
        );
    }
}