namespace FoodOrderSystem.Notification.Smtp
{
    public class SmtpConfiguration
    {
        public string ServerName { get; set; }
        
        public int Port  { get; set; }

        public string UserName  { get; set; }
        
        public string Password { get; set; }
    }
}