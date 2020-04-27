namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantFactory
    {
        Restaurant CreateWithName(string name);
    }
}
