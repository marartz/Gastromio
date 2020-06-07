namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantPickupInfoModel
    {
        public int AverageTime { get; set; }
        public decimal? MinimumOrderValue { get; set; }
        public decimal? MaximumOrderValue { get; set; }
        public string HygienicHandling { get; set; }
    }
}
