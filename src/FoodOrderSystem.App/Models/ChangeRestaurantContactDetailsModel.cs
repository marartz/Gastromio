using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeRestaurantContactDetailsModel
    {
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Imprint { get; set; }
        public string OrderEmailAddress { get; set; }
    }
}
