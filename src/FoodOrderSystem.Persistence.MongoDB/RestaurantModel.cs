using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public AddressModel Address { get; set; }
        
        public ContactInfoModel ContactInfo { get; set; }

        public List<OpeningPeriodModel> OpeningHours { get; set; }

        public PickupInfoModel PickupInfo { get; set; }
        
        public DeliveryInfoModel DeliveryInfo { get; set; }
        
        public ReservationInfoModel ReservationInfo { get; set; }
        
        public string HygienicHandling { get; set; }
        
        public List<Guid> Cuisines { get; set; }
        
        public List<Guid> PaymentMethods { get; set; }
        
        public List<Guid> Administrators { get; set; }
        
        public string ImportId { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool NeedsSupport { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public Guid CreatedBy { get; set; }
        
        public DateTime UpdatedOn { get; set; }
        
        public Guid UpdatedBy { get; set; }
    }
}