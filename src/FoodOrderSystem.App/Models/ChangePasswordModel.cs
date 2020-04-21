using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeUserPasswordModel
    {
        [Required]
        public string Password { get; set; }
    }
}
