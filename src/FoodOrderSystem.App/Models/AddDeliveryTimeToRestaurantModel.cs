using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddDeliveryTimeToRestaurantModel
    {
        public int DayOfWeek { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
