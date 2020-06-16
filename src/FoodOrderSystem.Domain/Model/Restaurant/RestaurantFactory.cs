using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class RestaurantFactory : IRestaurantFactory
    {
        public Result<Restaurant> CreateWithName(
            string name,
            UserId createdBy
        )
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                DateTime.UtcNow,
                createdBy,
                DateTime.UtcNow,
                createdBy
            );
            
            var tempResult = restaurant.ChangeName(name, createdBy);
            
            return tempResult.IsFailure ? tempResult.Cast<Restaurant>() : SuccessResult<Restaurant>.Create(restaurant);
        }
    }
}
