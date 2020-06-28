using System;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public class RestaurantImportLogLine
    {
        public DateTime Timestamp { get; set; }
        
        public RestaurantImportLogLineType Type { get; set; }
        
        public int RowIndex { get; set; }
        
        public string Message { get; set; }
    }
}