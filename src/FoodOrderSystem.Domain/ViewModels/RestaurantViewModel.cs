using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class RestaurantViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public AddressViewModel Address { get; set; }

        public ContactInfoViewModel ContactInfo { get; set; }

        public List<OpeningPeriodViewModel> OpeningHours { get; set; }

        public PickupInfoViewModel PickupInfo { get; set; }

        public DeliveryInfoViewModel DeliveryInfo { get; set; }

        public ReservationInfoViewModel ReservationInfo { get; set; }

        public List<CuisineViewModel> Cuisines { get; set; }

        public List<PaymentMethodViewModel> PaymentMethods { get; set; }

        public List<UserViewModel> Administrators { get; set; }

        public static RestaurantViewModel FromRestaurant(
            Restaurant restaurant,
            IDictionary<Guid, CuisineViewModel> allCuisines,
            IDictionary<Guid, PaymentMethodViewModel> allPaymentMethods,
            IUserRepository userRepository)
        {
            string image = null;
            if (restaurant.Image != null && restaurant.Image.Length != 0)
            {
                var sb = new StringBuilder();
                sb.Append("data:image/png;base64,");
                sb.Append(Convert.ToBase64String(restaurant.Image));
                image = sb.ToString();
            }

            return new RestaurantViewModel
            {
                Id = restaurant.Id.Value,
                Name = restaurant.Name,
                Image = image,
                Address = restaurant.Address != null
                    ? AddressViewModel.FromAddress(restaurant.Address)
                    : new AddressViewModel(),
                ContactInfo = restaurant.ContactInfo != null
                    ? ContactInfoViewModel.FromContactInfo(restaurant.ContactInfo)
                    : new ContactInfoViewModel(),
                OpeningHours = restaurant.OpeningHours != null
                    ? restaurant.OpeningHours.Select(OpeningPeriodViewModel.FromOpeningPeriod).ToList()
                    : new List<OpeningPeriodViewModel>(),
                PickupInfo = restaurant.PickupInfo != null
                    ? PickupInfoViewModel.FromPickupInfo(restaurant.PickupInfo)
                    : new PickupInfoViewModel(),
                DeliveryInfo = restaurant.DeliveryInfo != null
                    ? DeliveryInfoViewModel.FromDeliveryInfo(restaurant.DeliveryInfo)
                    : new DeliveryInfoViewModel(),
                ReservationInfo = restaurant.ReservationInfo != null
                    ? ReservationInfoViewModel.FromReservationInfo(restaurant.ReservationInfo)
                    : new ReservationInfoViewModel(),
                Cuisines = restaurant.Cuisines != null
                    ? restaurant.Cuisines.Select(en => RetrieveCuisineModel(allCuisines, en.Value)).ToList()
                    : new List<CuisineViewModel>(),
                PaymentMethods = restaurant.PaymentMethods != null
                    ? restaurant.PaymentMethods.Select(en => RetrievePaymentMethodModel(allPaymentMethods, en.Value))
                        .ToList()
                    : new List<PaymentMethodViewModel>(),
                Administrators = restaurant.Administrators != null
                    ? restaurant.Administrators.Select(en => RetrieveUserModel(userRepository, en)).ToList()
                    : new List<UserViewModel>()
            };
        }

        public static CuisineViewModel RetrieveCuisineModel(IDictionary<Guid, CuisineViewModel> allCuisines,
            Guid cuisineId)
        {
            return allCuisines.TryGetValue(cuisineId, out var model) ? model : null;
        }

        public static PaymentMethodViewModel RetrievePaymentMethodModel(
            IDictionary<Guid, PaymentMethodViewModel> allPaymentMethods, Guid paymentMethodId)
        {
            return allPaymentMethods.TryGetValue(paymentMethodId, out var model) ? model : null;
        }

        public static UserViewModel RetrieveUserModel(IUserRepository userRepository, UserId userId)
        {
            var user = userRepository.FindByUserIdAsync(userId).Result;
            if (user == null)
                return null;
            return UserViewModel.FromUser(user);
        }
    }
}