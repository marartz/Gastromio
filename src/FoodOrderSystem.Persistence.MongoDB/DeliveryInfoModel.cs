namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DeliveryInfoModel
    {
        public int AverageTime { get; set; }

        public double? MinimumOrderValue { get; set; }
        
        public double? MaximumOrderValue { get; set; }
        
        public double? Costs { get; set; }
        
        public string HygienicHandling { get; set; }
    }
}