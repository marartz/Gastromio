namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantDeliveryInfoModel
    {
        public int AverageTime { get; set; }
        public decimal? MinimumOrderValue { get; set; }
        public decimal? MaximumOrderValue { get; set; }
        public decimal? Costs { get; set; }
        public string HygienicHandling { get; set; }
    }
}
