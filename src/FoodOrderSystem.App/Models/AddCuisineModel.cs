using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddCuisineModel
    {
        [Required]
        public string Name { get; set; }

        public string Image { get; set; }
    }
}
