using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddDeliveryTimeToRestaurantModel
    {
        [Required]
        public int DayOfWeek { get; set; }
        [Required]
        public int Start { get; set; }
        [Required]
        public int End { get; set; }
    }
}
