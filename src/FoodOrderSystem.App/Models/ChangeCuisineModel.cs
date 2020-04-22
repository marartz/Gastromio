using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeCuisineModel
    {
        [Required]
        public string Name { get; set; }
    }
}
