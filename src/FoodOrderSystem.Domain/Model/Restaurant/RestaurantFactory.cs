using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class RestaurantFactory : IRestaurantFactory
    {
        public Result<Restaurant> CreateWithName(string name)
        {
            var restaurant = new Restaurant(new RestaurantId(Guid.NewGuid()));
            var tempResult = restaurant.ChangeName(name);
            return tempResult.IsFailure ? tempResult.Cast<Restaurant>() : SuccessResult<Restaurant>.Create(restaurant);
        }
    }
}
