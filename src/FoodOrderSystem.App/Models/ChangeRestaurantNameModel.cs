using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantNameModel
    {
        [Required]
        public string Name { get; set; }
    }
}
