namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DeliveryInfoModel
    {
        public bool Enabled { get; set; }
        
        public int? AverageTime { get; set; }

        public double? MinimumOrderValue { get; set; }
        
        public double? MaximumOrderValue { get; set; }
        
        public double? Costs { get; set; }
    }
}