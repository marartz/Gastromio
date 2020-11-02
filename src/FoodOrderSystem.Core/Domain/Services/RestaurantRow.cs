using System;

namespace FoodOrderSystem.Core.Domain.Services
{
    public class RestaurantRow
    {
        public string ImportId { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public string ResponsiblePerson { get; set; }
        public string OrderEmailAddress { get; set; }

        public string OpeningHoursMonday { get; set; }
        public string OpeningHoursTuesday { get; set; }
        public string OpeningHoursWednesday { get; set; }
        public string OpeningHoursThursday { get; set; }
        public string OpeningHoursFriday { get; set; }
        public string OpeningHoursSaturday { get; set; }
        public string OpeningHoursSunday { get; set; }
        
        public string OrderTypes { get; set; }
        public TimeSpan? AverageTime { get; set; }
        public double? MinimumOrderValuePickup { get; set; }
        public double? MinimumOrderValueDelivery { get; set; }
        public double? DeliveryCosts { get; set; }

        public string HygienicHandling { get; set; }

        public string Cuisines { get; set; }

        public string PaymentMethods { get; set; }

        public string AdministratorUserEmailAddress { get; set; }
        
        public bool IsActive { get; set; }
    }
}