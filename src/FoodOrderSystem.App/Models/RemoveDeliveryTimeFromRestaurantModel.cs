using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveDeliveryTimeFromRestaurantModel
    {
        public int DayOfWeek { get; set; }
        public int Start { get; set; }
    }
}
