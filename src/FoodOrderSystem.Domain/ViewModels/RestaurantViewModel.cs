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

        public AddressViewModel Address { get; set; }

        public ContactInfoViewModel ContactInfo { get; set; }

        public List<OpeningPeriodViewModel> OpeningHours { get; set; }
        
        public string OpeningHoursText { get; set; }

        public PickupInfoViewModel PickupInfo { get; set; }

        public DeliveryInfoViewModel DeliveryInfo { get; set; }

        public ReservationInfoViewModel ReservationInfo { get; set; }
        
        public string HygienicHandling { get; set; }

        public List<CuisineViewModel> Cuisines { get; set; }

        public List<PaymentMethodViewModel> PaymentMethods { get; set; }

        public List<UserViewModel> Administrators { get; set; }

        public static RestaurantViewModel FromRestaurant(
            Restaurant restaurant,
            IDictionary<Guid, CuisineViewModel> allCuisines,
            IDictionary<Guid, PaymentMethodViewModel> allPaymentMethods,
            IUserRepository userRepository)
        {
            var openingHoursText = GenerateOpeningHoursText(restaurant); 

            return new RestaurantViewModel
            {
                Id = restaurant.Id.Value,
                Name = restaurant.Name,
                Address = restaurant.Address != null
                    ? AddressViewModel.FromAddress(restaurant.Address)
                    : new AddressViewModel(),
                ContactInfo = restaurant.ContactInfo != null
                    ? ContactInfoViewModel.FromContactInfo(restaurant.ContactInfo)
                    : new ContactInfoViewModel(),
                OpeningHours = restaurant.OpeningHours != null
                    ? restaurant.OpeningHours.Select(OpeningPeriodViewModel.FromOpeningPeriod).ToList()
                    : new List<OpeningPeriodViewModel>(),
                OpeningHoursText = openingHoursText,
                PickupInfo = restaurant.PickupInfo != null
                    ? PickupInfoViewModel.FromPickupInfo(restaurant.PickupInfo)
                    : new PickupInfoViewModel(),
                DeliveryInfo = restaurant.DeliveryInfo != null
                    ? DeliveryInfoViewModel.FromDeliveryInfo(restaurant.DeliveryInfo)
                    : new DeliveryInfoViewModel(),
                ReservationInfo = restaurant.ReservationInfo != null
                    ? ReservationInfoViewModel.FromReservationInfo(restaurant.ReservationInfo)
                    : new ReservationInfoViewModel(),
                HygienicHandling = restaurant.HygienicHandling,
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

        private static CuisineViewModel RetrieveCuisineModel(IDictionary<Guid, CuisineViewModel> allCuisines,
            Guid cuisineId)
        {
            return allCuisines.TryGetValue(cuisineId, out var model) ? model : null;
        }

        private static PaymentMethodViewModel RetrievePaymentMethodModel(
            IDictionary<Guid, PaymentMethodViewModel> allPaymentMethods, Guid paymentMethodId)
        {
            return allPaymentMethods.TryGetValue(paymentMethodId, out var model) ? model : null;
        }

        private static UserViewModel RetrieveUserModel(IUserRepository userRepository, UserId userId)
        {
            var user = userRepository.FindByUserIdAsync(userId).Result;
            if (user == null)
                return null;
            return UserViewModel.FromUser(user);
        }

        private static string GenerateOpeningHoursText(Restaurant restaurant)
        {
            if (restaurant.OpeningHours == null)
                return string.Empty;

            var sb = new StringBuilder();

            var openingPeriodsPerDay = new List<List<OpeningPeriod>>();
            for (var dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                openingPeriodsPerDay.Add(new List<OpeningPeriod>());
            }
            
            foreach (var openingPeriod in restaurant.OpeningHours.OrderBy(en => en.DayOfWeek).ThenBy(en => en.Start))
            {
                openingPeriodsPerDay[openingPeriod.DayOfWeek].Add(openingPeriod);
            }

            var first = true;
            var startDayOfWeek = 0;
            var openingPeriods = openingPeriodsPerDay[startDayOfWeek];
            
            for (var dayOfWeek = 1; dayOfWeek < 7; dayOfWeek++)
            {
                if (!OpeningPeriodsEquals(openingPeriods, openingPeriodsPerDay[dayOfWeek]))
                {
                    if (openingPeriods.Count > 0)
                    {
                        if (!first)
                            sb.Append("; ");
                        WriteOpeningPeriods(sb, startDayOfWeek, dayOfWeek - 1, openingPeriods);
                        first = false;
                    }

                    startDayOfWeek = dayOfWeek;
                    openingPeriods = openingPeriodsPerDay[dayOfWeek];
                }
            }

            if (openingPeriods.Count > 0)
            {
                if (!first)
                    sb.Append("; ");
                WriteOpeningPeriods(sb, startDayOfWeek, 6, openingPeriods);
            }

            return sb.ToString();
        }

        private static void WriteOpeningPeriods(StringBuilder sb, int startDayOfWeek, int endDayOfWeek,
            IEnumerable<OpeningPeriod> openingPeriods)
        {
            if (endDayOfWeek > startDayOfWeek)
            {
                WriteDayOfWeekName(sb, startDayOfWeek);
                sb.Append("-");
                WriteDayOfWeekName(sb, endDayOfWeek);
            }
            else
            {
                WriteDayOfWeekName(sb, startDayOfWeek);
            }

            sb.Append(" ");

            var first = true;
            foreach (var openingPeriod in openingPeriods)
            {
                if (!first)
                    sb.Append(" ");

                WriteTime(sb, openingPeriod.Start);
                sb.Append("-");
                WriteTime(sb, openingPeriod.End);
                
                first = false;
            }
        }

        private static void WriteDayOfWeekName(StringBuilder sb, int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 0:
                    sb.Append("Mo");
                    break;
                case 1:
                    sb.Append("Di");
                    break;
                case 2:
                    sb.Append("Mi");
                    break;
                case 3:
                    sb.Append("Do");
                    break;
                case 4:
                    sb.Append("Fr");
                    break;
                case 5:
                    sb.Append("Sa");
                    break;
                case 6:
                    sb.Append("So");
                    break;
            }
        }

        private static void WriteTime(StringBuilder sb, TimeSpan time)
        {
            sb.Append(time.Hours.ToString("00"));
            sb.Append(":");
            sb.Append(time.Minutes.ToString("00"));
        }

        private static bool OpeningPeriodsEquals(List<OpeningPeriod> op1, List<OpeningPeriod> op2)
        {
            if (op1 == null && op2 == null)
                return true;
            if (op1 == null || op2 == null)
                return false;
            
            if (op1.Count != op2.Count)
                return false;
            for (var i = 0; i < op1.Count; i++)
            {
                if (op1[i].Start != op2[i].Start)
                    return false;
                if (op1[i].End != op2[i].End)
                    return false;
            }
            return true;
        }
    }
}