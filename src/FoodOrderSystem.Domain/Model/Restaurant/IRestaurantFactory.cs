namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantFactory
    {
        Result<Restaurant> CreateWithName(string name);
    }
}
