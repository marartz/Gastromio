namespace FoodOrderSystem.Domain.Adapters.Notification
{
    public class NotificationResponse
    {
        public NotificationResponse(
            bool success,
            string message
        )
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        
        public string Message { get; }
    }
}