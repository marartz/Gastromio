using System;

namespace Gastromio.App.Models
{
    public class AddDishCategoryToRestaurantModel
    {
        public string Name { get; set; }
        
        public Guid? AfterCategoryId { get; set; }
    }
}
