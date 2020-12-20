using System;

namespace Gastromio.Core.Domain.Services
{
    public class ImportLogLine
    {
        public ImportLogLine(DateTime timestamp, ImportLogLineType type, int rowIndex, string message)
        {
            Timestamp = timestamp;
            Type = type;
            RowIndex = rowIndex;
            Message = message;
        }
        
        public DateTime Timestamp { get; }
        
        public ImportLogLineType Type { get; }
        
        public int RowIndex { get; }
        
        public string Message { get; }
    }
}