using System;
using System.Collections.Generic;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Cuisine;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class RestaurantFactory : IRestaurantFactory
    {
        public Result<Restaurant> Create(
            string name,
            Address address,
            ContactInfo contactInfo,
            IList<OpeningPeriod> openingHours,
            PickupInfo pickupInfo,
            DeliveryInfo deliveryInfo,
            ReservationInfo reservationInfo,
            string hygienicHandling,
            ISet<CuisineId> cuisines,
            ISet<PaymentMethodId> paymentMethods,
            ISet<UserId> administrators,
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
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            tempResult = restaurant.ChangeAddress(address, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            tempResult = restaurant.ChangeContactInfo(contactInfo, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            if (openingHours != null)
            {
                foreach (var openingPeriod in openingHours)
                {
                    tempResult = restaurant.AddOpeningPeriod(openingPeriod, createdBy);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Restaurant>();
                }
            }

            tempResult = restaurant.ChangePickupInfo(pickupInfo, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            tempResult = restaurant.ChangeDeliveryInfo(deliveryInfo, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            tempResult = restaurant.ChangeReservationInfo(reservationInfo, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            tempResult = restaurant.ChangeHygienicHandling(hygienicHandling, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Restaurant>();

            if (cuisines != null)
            {
                foreach (var cuisine in cuisines)
                {
                    tempResult = restaurant.AddCuisine(cuisine, createdBy);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Restaurant>();
                }
            }

            if (paymentMethods != null)
            {
                foreach (var paymentMethod in paymentMethods)
                {
                    tempResult = restaurant.AddPaymentMethod(paymentMethod, createdBy);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Restaurant>();
                }
            }

            if (administrators != null)
            {
                foreach (var administrator in administrators)
                {
                    tempResult = restaurant.AddAdministrator(administrator, createdBy);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Restaurant>();
                }
            }

            return SuccessResult<Restaurant>.Create(restaurant);
        }

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
