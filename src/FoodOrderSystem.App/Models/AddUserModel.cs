using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddUserModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
