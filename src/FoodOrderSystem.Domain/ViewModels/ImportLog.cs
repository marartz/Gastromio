using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class ImportLog
    {
        private readonly object lockObj = new object();
        private readonly List<ImportLogLine> lines = new List<ImportLogLine>();
        
        public void AddLine(ImportLogLineType logLineType, int rowIndex, string message, params object[] args)
        {
            lock (lockObj)
            {
                lines.Add(new ImportLogLine
                {
                    Timestamp = DateTime.UtcNow,
                    Type = logLineType,
                    RowIndex = rowIndex,
                    Message = string.Format(message, args)
                });
            }
        }
        
        public IReadOnlyCollection<ImportLogLine> Lines
        {
            get
            {
                lock (lockObj)
                {
                    return new ReadOnlyCollection<ImportLogLine>(lines);
                }
            }
        }
    }
}