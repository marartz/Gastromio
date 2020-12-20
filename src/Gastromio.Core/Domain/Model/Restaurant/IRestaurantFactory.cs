using System.Collections.Generic;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Cuisine;
using Gastromio.Core.Domain.Model.PaymentMethod;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Restaurant
{
    public interface IRestaurantFactory
    {
        Result<Restaurant> Create(
            string name,
            Address address,
            ContactInfo contactInfo,
            IEnumerable<RegularOpeningDay> regularOpeningDays,
            IEnumerable<DeviatingOpeningDay> deviatingOpeningDays,
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