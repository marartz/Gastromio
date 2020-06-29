using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class ImportLogLine
    {
        public DateTime Timestamp { get; set; }
        
        public ImportLogLineType Type { get; set; }
        
        public int RowIndex { get; set; }
        
        public string Message { get; set; }
    }
}