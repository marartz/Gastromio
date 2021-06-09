using System.Collections.Generic;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public interface IRestaurantFactory
    {
        Restaurant Create(
            string name,
            Address address,
            ContactInfo contactInfo,
            RegularOpeningDays regularOpeningDays,
            DeviatingOpeningDays deviatingOpeningDays,
            PickupInfo pickupInfo,
            DeliveryInfo deliveryInfo,
            ReservationInfo reservationInfo,
            string hygienicHandling,
            ISet<CuisineId> cuisines,
            ISet<PaymentMethodId> paymentMethods,
            ISet<UserId> administrators,
            UserId createdBy
        );

        Restaurant CreateWithName(
            string name,
            UserId createdBy
        );
    }
}
