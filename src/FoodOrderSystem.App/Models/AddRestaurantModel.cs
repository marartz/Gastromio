using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddRestaurantModel
    {
        [Required]
        public string Name { get; set; }
    }
}
