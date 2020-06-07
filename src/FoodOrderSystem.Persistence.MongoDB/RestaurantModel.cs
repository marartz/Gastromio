using System;
using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Image { get; set; }

        public AddressModel Address { get; set; }
        
        public ContactInfoModel ContactInfo { get; set; }

        public List<OpeningPeriodModel> OpeningHours { get; set; }

        public PickupInfoModel PickupInfo { get; set; }
        
        public DeliveryInfoModel DeliveryInfo { get; set; }
        
        public ReservationInfoModel ReservationInfo { get; set; }
        
        public List<Guid> Cuisines { get; set; }
        
        public List<Guid> PaymentMethods { get; set; }
        
        public List<Guid> Administrators { get; set; }
    }
}