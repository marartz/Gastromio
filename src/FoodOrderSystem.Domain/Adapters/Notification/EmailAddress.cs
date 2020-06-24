namespace FoodOrderSystem.Domain.Adapters.Notification
{
    public class EmailAddress
    {
        public EmailAddress(string name, string email)
        {
            Name = name;
            Email = email;
        }
        
        public string Name { get; }

        public string Email { get; }
    }
}