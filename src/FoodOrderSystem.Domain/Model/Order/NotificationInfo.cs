using System;

namespace FoodOrderSystem.Domain.Model.Order
{
    public class NotificationInfo
    {
        public NotificationInfo(
            bool status,
            int attempt,
            string message,
            DateTime timestamp
        )
        {
            Status = status;
            Attempt = attempt;
            Message = message;
            Timestamp = timestamp;
        }

        public bool Status { get; }
        
        public int Attempt { get; }
        
        public string Message { get; }
        
        public DateTime Timestamp { get; }
    }
}