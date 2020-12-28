namespace Gastromio.Core.Application.Ports.Notification
{
    public class MobileNotificationRequest
    {
        public MobileNotificationRequest(string from, string to, string text)
        {
            From = from;
            To = to;
            Text = text;
        }
        
        public string From { get; }
        public string To { get; }
        public string Text { get; }
    }
}