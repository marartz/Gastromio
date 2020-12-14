using System.Collections.Generic;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Cuisine;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public interface IRestaurantFactory
    {
        Result<Restaurant> Create(
            string name,
            Address address,
            ContactInfo contactInfo,
            IList<RegularOpeningPeriod> regularOpeningPeriods,
            IList<DeviatingOpeningPeriod> deviatingOpeningPeriods,
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