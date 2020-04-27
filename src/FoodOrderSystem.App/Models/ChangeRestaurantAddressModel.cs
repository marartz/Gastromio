using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantAddressModel
    {
        [Required]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
    }
}
