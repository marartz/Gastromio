using FoodOrderSystem.Domain.Model.PaymentMethod;
using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class RestaurantFactory : IRestaurantFactory
    {
        public Restaurant CreateWithName(string name)
        {
            return new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                name,
                null,
                new Address(null, null, null),
                new List<DeliveryTime>(),
                0,
                0,
                null,
                null,
                null,
                null,
                new HashSet<PaymentMethodId>()
            );
        }
    }
}
