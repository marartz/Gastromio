using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveDeliveryTimeFromRestaurantModel
    {
        [Required]
        public int DayOfWeek { get; set; }
        [Required]
        public int Start { get; set; }
    }
}
