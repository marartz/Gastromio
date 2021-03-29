namespace Gastromio.Core.Application.Ports.Notification
{
    public class MobileNotificationResponse
    {
        public MobileNotificationResponse(
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