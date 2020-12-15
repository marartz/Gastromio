using System;

namespace FoodOrderSystem.App.Models
{
    public class AddOrChangeExternalMenuOfRestaurantModel
    {
        public Guid ExternalMenuId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
        
        public string Url { get; set; }
    }
}