namespace FoodOrderSystem.Core.Domain.Services
{
    public class DishRow
    {
        public string Category { get; set; }
        
        public string DishName { get; set; }
        
        public string Description { get; set; }
        
        public string ProductInfo { get; set; }
        
        public string VariantName { get; set; }
        
        public double? VariantPrice { get; set; }
        
        public string RestaurantImportId { get; set; }
    }
}