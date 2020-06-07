namespace FoodOrderSystem.Persistence.MongoDB
{
    public class PickupInfoModel
    {
        public int AverageTime { get; set; }

        public decimal? MinimumOrderValue { get; set; }
        
        public decimal? MaximumOrderValue { get; set; }
        
        public string HygienicHandling { get; set; }
    }
}