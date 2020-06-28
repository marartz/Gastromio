using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FoodOrderSystem.Domain.Commands.ImportRestaurantData
{
    public class RestaurantImportLog
    {
        private readonly object lockObj = new object();
        private readonly List<RestaurantImportLogLine> lines = new List<RestaurantImportLogLine>();
        
        public void AddLine(RestaurantImportLogLineType logLineType, int rowIndex, string message, params object[] args)
        {
            lock (lockObj)
            {
                lines.Add(new RestaurantImportLogLine
                {
                    Timestamp = DateTime.UtcNow,
                    Type = logLineType,
                    RowIndex = rowIndex,
                    Message = string.Format(message, args)
                });
            }
        }
        
        public IReadOnlyCollection<RestaurantImportLogLine> Lines
        {
            get
            {
                lock (lockObj)
                {
                    return new ReadOnlyCollection<RestaurantImportLogLine>(lines);
                }
            }
        }
    }
}