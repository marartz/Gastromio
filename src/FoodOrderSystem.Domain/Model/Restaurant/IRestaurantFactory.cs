using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantFactory
    {
        Result<Restaurant> CreateWithName(
            string name,
            UserId createdBy
        );
    }
}
