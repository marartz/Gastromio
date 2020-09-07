using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class RestaurantDTO
    {
        internal RestaurantDTO(Restaurant restaurant,
            IDictionary<Guid, CuisineDTO> allCuisines,
            IDictionary<Guid, PaymentMethodDTO> allPaymentMethods,
            IUserRepository userRepository,
            IRestaurantImageRepository restaurantImageRepository)
        {
            var openingHoursText = GenerateOpeningHoursText(restaurant);
            var openingHoursTodayText = GenerateOpeningHoursTodayText(restaurant);

            Id = restaurant.Id.Value;
            Name = restaurant.Name;
            Address = restaurant.Address;
            ContactInfo = restaurant.ContactInfo;
            ImageTypes = RetrieveImageTypes(restaurantImageRepository, restaurant.Id);
            OpeningHours =
                new ReadOnlyCollection<OpeningPeriodDTO>(restaurant.OpeningHours.Select(en => new OpeningPeriodDTO(en))
                    .ToList());
            OpeningHoursText = openingHoursText;
            OpeningHoursTodayText = openingHoursTodayText;
            PickupInfo = restaurant.PickupInfo;
            DeliveryInfo = restaurant.DeliveryInfo;
            ReservationInfo = restaurant.ReservationInfo;
            HygienicHandling = restaurant.HygienicHandling;
            Cuisines = restaurant.Cuisines != null
                ? restaurant.Cuisines.Select(en => RetrieveCuisineModel(allCuisines, en.Value)).ToList()
                : new List<CuisineDTO>();
            CuisinesText = restaurant.Cuisines != null
                ? string.Join(" • ", Cuisines.Select(en => en.Name))
                : "";
            PaymentMethods = restaurant.PaymentMethods != null
                ? restaurant.PaymentMethods.Select(en => RetrievePaymentMethodModel(allPaymentMethods, en.Value))
                    .ToList()
                : new List<PaymentMethodDTO>();
            Administrators = restaurant.Administrators != null
                ? restaurant.Administrators.Select(en => RetrieveUserModel(userRepository, en)).ToList()
                : new List<UserDTO>();
            IsActive = restaurant.IsActive;
            NeedsSupport = restaurant.NeedsSupport;
        }

        public Guid Id { get; }

        public string Name { get; }

        public Address Address { get; }

        public ContactInfo ContactInfo { get; }

        public IReadOnlyCollection<string> ImageTypes { get; }

        public IReadOnlyCollection<OpeningPeriodDTO> OpeningHours { get; }

        public string OpeningHoursText { get; }

        public string OpeningHoursTodayText { get; }

        public PickupInfo PickupInfo { get; }

        public DeliveryInfo DeliveryInfo { get; }

        public ReservationInfo ReservationInfo { get; }

        public string HygienicHandling { get; }

        public IReadOnlyCollection<CuisineDTO> Cuisines { get; }

        public string CuisinesText { get; }

        public IReadOnlyCollection<PaymentMethodDTO> PaymentMethods { get; }

        public IReadOnlyCollection<UserDTO> Administrators { get; }

        public bool IsActive { get; }

        public bool NeedsSupport { get; }

        private static CuisineDTO RetrieveCuisineModel(IDictionary<Guid, CuisineDTO> allCuisines,
            Guid cuisineId)
        {
            return allCuisines.TryGetValue(cuisineId, out var model) ? model : null;
        }

        private static PaymentMethodDTO RetrievePaymentMethodModel(
            IDictionary<Guid, PaymentMethodDTO> allPaymentMethods, Guid paymentMethodId)
        {
            return allPaymentMethods.TryGetValue(paymentMethodId, out var model) ? model : null;
        }

        private static UserDTO RetrieveUserModel(IUserRepository userRepository, UserId userId)
        {
            var user = userRepository.FindByUserIdAsync(userId).GetAwaiter().GetResult();
            return user != null ? new UserDTO(user) : null;
        }

        private static List<string> RetrieveImageTypes(IRestaurantImageRepository restaurantImageRepository,
            RestaurantId restaurantId)
        {
            return restaurantImageRepository.FindTypesByRestaurantIdAsync(restaurantId).GetAwaiter().GetResult()
                .ToList();
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
                        WriteOpeningPeriodsForDays(sb, startDayOfWeek, dayOfWeek - 1, openingPeriods);
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
                WriteOpeningPeriodsForDays(sb, startDayOfWeek, 6, openingPeriods);
            }

            return sb.ToString();
        }

        private static string GenerateOpeningHoursTodayText(Restaurant restaurant)
        {
            if (restaurant.OpeningHours == null)
                return "Geschlossen";

            var now = DateTime.Now;
            var dayOfWeek = ((int)now.DayOfWeek - 1) % 7; // DayOfWeek starts with Sunday 
            if (dayOfWeek < 0)
            {
                dayOfWeek += 7;
            }
            if (now.Hour < 4)
            {
                dayOfWeek = (dayOfWeek - 1) % 7;
                if (dayOfWeek < 0)
                {
                    dayOfWeek += 7;
                }
            }

            var openingPeriods = restaurant.OpeningHours.Where(en => en.DayOfWeek == dayOfWeek).OrderBy(en => en.Start)
                .ToList();

            if (openingPeriods.Any())
            {
                var sb = new StringBuilder();
                sb.Append("Geöffnet ");
                WriteOpeningPeriods(sb, openingPeriods);
                return sb.ToString();
            }
            else
            {
                return "Geschlossen";
            }
        }

        private static void WriteOpeningPeriodsForDays(StringBuilder sb, int startDayOfWeek, int endDayOfWeek,
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
            
            WriteOpeningPeriods(sb, openingPeriods);
        }

        private static void WriteOpeningPeriods(StringBuilder sb, IEnumerable<OpeningPeriod> openingPeriods)
        {
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