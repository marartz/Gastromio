using System;
using System.Collections.Generic;
using System.Linq;
using Gastromio.Core.Domain.Model.Cuisines;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class RestaurantFactory : IRestaurantFactory
    {
        public Restaurant Create(
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
        )
        {
            var restaurant = CreateRestaurant(name, createdBy);
            restaurant.ChangeAddress(address, createdBy);
            restaurant.ChangeContactInfo(contactInfo, createdBy);
            restaurant.ChangeRegularOpeningDays(regularOpeningDays, createdBy);
            restaurant.ChangeDeviatingOpeningDays(deviatingOpeningDays, createdBy);
            restaurant.ChangePickupInfo(pickupInfo, createdBy);
            restaurant.ChangeDeliveryInfo(deliveryInfo, createdBy);
            restaurant.ChangeReservationInfo(reservationInfo, createdBy);
            restaurant.ChangeHygienicHandling(hygienicHandling, createdBy);

            if (cuisines != null)
            {
                foreach (var cuisine in cuisines)
                {
                    restaurant.AddCuisine(cuisine, createdBy);
                }
            }

            if (paymentMethods != null)
            {
                foreach (var paymentMethod in paymentMethods)
                {
                    restaurant.AddPaymentMethod(paymentMethod, createdBy);
                }
            }

            if (administrators != null)
            {
                foreach (var administrator in administrators)
                {
                    restaurant.AddAdministrator(administrator, createdBy);
                }
            }

            return restaurant;
        }

        public Restaurant CreateWithName(
            string name,
            UserId createdBy
        )
        {
            return CreateRestaurant(name, createdBy);
        }

        private static Restaurant CreateRestaurant(string name, UserId createdBy)
        {
            var restaurant = new Restaurant(
                new RestaurantId(Guid.NewGuid()),
                name,
                null,
                new Address(null, null, null),
                new ContactInfo(null, null, null, null, null, null, false),
                new RegularOpeningDays(Enumerable.Empty<RegularOpeningDay>()),
                new DeviatingOpeningDays(Enumerable.Empty<DeviatingOpeningDay>()),
                new PickupInfo(false, 0, null, null),
                new DeliveryInfo(false, 0, null, null, null),
                new ReservationInfo(false, null),
                null,
                new HashSet<CuisineId>(),
                new HashSet<PaymentMethodId>(),
                new HashSet<UserId>(),
                null,
                false,
                false,
                SupportedOrderMode.OnlyPhone,
                new DishCategories(Enumerable.Empty<DishCategory>()),
                new ExternalMenus(Enumerable.Empty<ExternalMenu>()),
                DateTimeOffset.UtcNow,
                createdBy,
                DateTimeOffset.UtcNow,
                createdBy
            );

            return restaurant;
        }
    }
}
