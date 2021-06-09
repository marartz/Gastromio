using System.Collections.Generic;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Moq;
using Moq.Language.Flow;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class RestaurantFactoryMock : Mock<IRestaurantFactory>
    {
        public RestaurantFactoryMock(MockBehavior behavior) : base(behavior)
        {
        }

        public ISetup<IRestaurantFactory, Restaurant> SetupCreate(
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
            UserId createBy
        )
        {
            return Setup(m => m.Create(
                name,
                address,
                contactInfo,
                regularOpeningDays,
                deviatingOpeningDays,
                pickupInfo,
                deliveryInfo,
                reservationInfo,
                hygienicHandling,
                cuisines,
                paymentMethods,
                administrators,
                createBy)
            );
        }

        public ISetup<IRestaurantFactory, Restaurant> SetupCreateWithName(
            string name,
            UserId createdBy
        )
        {
            return Setup(m => m.CreateWithName(name, createdBy));
        }
    }
}
