using System;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        
        public CustomerInfoModel CustomerInfo { get; set; }
        
        public CartInfoModel CartInfo { get; set; }
        
        public string Comments { get; set; }
    }
}