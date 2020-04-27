using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantDeliveryDataModel
    {
        public decimal MinimumOrderValue { get; set; }
        public decimal DeliveryCosts { get; set; }
    }
}
