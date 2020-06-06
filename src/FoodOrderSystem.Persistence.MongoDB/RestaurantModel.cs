using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class RestaurantModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Image { get; set; }

        public string AddressStreet { get; set; }

        public string AddressZipCode { get; set; }

        public string AddressCity { get; set; }

        public List<DeliveryTimeModel> DeliveryTimes { get; set; }

        public decimal MinimumOrderValue { get; set; }

        public decimal DeliveryCosts { get; set; }

        public string Phone { get; set; }

        public string WebSite { get; set; }

        public string Imprint { get; set; }

        public string OrderEmailAddress { get; set; }
        
        public List<Guid> Cuisines { get; set; }
        
        public List<Guid> PaymentMethods { get; set; }
        
        public List<Guid> Administrators { get; set; }
    }
}