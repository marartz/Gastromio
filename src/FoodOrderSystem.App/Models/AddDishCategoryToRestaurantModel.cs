using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddDishCategoryToRestaurantModel
    {
        [Required]
        public string Name { get; set; }
    }
}
