using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantImageModel
    {
        [Required]
        public string Image { get; set; }
    }
}
