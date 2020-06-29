using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantFactory
    {
        Result<Restaurant> Create(
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
            UserId createBy
        );

        Result<Restaurant> CreateWithName(
            string name,
            UserId createdBy
        );
    }
}